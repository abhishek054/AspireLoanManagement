using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Repository.Loan;
using AspireLoanManagement.Repository.Repayment;
using AspireLoanManagement.Utility.Cache;
using AspireLoanManagement.Utility.CommonEntities;
using AspireLoanManagement.Utility.Logger;
using AutoMapper;

namespace AspireLoanManagement.Business.Repayment
{
    public class RepaymentService: IRepaymentService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IAspireLogger _logger;
        private readonly IAspireCacheService _cache;
        private readonly IRepaymentRepository _repaymentRepository;
        
        public RepaymentService(ILoanRepository loanRepository, IAspireLogger logger, IAspireCacheService cache, IRepaymentRepository repaymentRepository)
        {
            _loanRepository = loanRepository;
            _logger = logger;
            _cache = cache;
            _repaymentRepository = repaymentRepository;
        }

        public async Task<bool> SettleRepayment(RepaymentModelVM repayment)
        {
            // Get the loan details from DB
            var loan = await _loanRepository.GetLoanByIdAsync(repayment.LoanID);

            if (loan.UserId != repayment.UserId)
            {
                throw new Exception("Invalid settlement");
            }

            if (loan.Status != LoanStatus.Approved)
            {
                _logger.Log(LogLevel.Info, $"Loan is not yet approved for loan: {repayment.LoanID}");
                return false;
            }

            // Handle only unpaid repayments
            var activeRepayments = loan.Repayments.Where(x => x.Status != RepaymentStatus.Paid).ToList();
            if (activeRepayments == null || activeRepayments.Count == 0)
            {
                throw new Exception("No repayment required");
            }

            var latestActiveRepayment = activeRepayments.OrderBy(x => x.ExpectedRepaymentDate).First();
            var repaymentAmount = latestActiveRepayment.PendingAmount;
            var pendingLoanAmount = activeRepayments.Sum(x => x.PendingAmount);
            if (repayment.RepaymentAmount > pendingLoanAmount)
            {
                // User trying to pay more than overall pending loan amount
                throw new Exception("Cannot pay more than pending loan amount");
            }

            if (repayment.RepaymentAmount < latestActiveRepayment.PendingAmount)
            {
                // User trying to pay an amount less than the repayment installment
                throw new Exception("Proposed payment amount is lesser than expected amount");
            }

            // Repayment of the earliest installment
            await _repaymentRepository.SettleRepayment(latestActiveRepayment.Id);

            // If the settled repayment was the last active repayment of the loan, settle the Loan and exit
            if (activeRepayments.Count == 1)
            {
                await _loanRepository.SettleLoan(loan.Id);
                _cache.Remove(loan.Id.ToString());
                return true;
            }

            // Update the next Repayments if necessary
            if (repayment.RepaymentAmount > repaymentAmount)
            {
                // Tracking other active repayments in order of expected payment date
                activeRepayments = activeRepayments.Where(x => x.Id != latestActiveRepayment.Id).OrderBy(x => x.ExpectedRepaymentDate).ToList();
                decimal extraAmount = repayment.RepaymentAmount - repaymentAmount;
                int repaymentIndex = 0; // To track other active repayments based on index

                while (extraAmount > 0 && repaymentIndex < activeRepayments.Count)
                {
                    var nextRepayment = activeRepayments[repaymentIndex++];
                    if (extraAmount < nextRepayment.PendingAmount)
                    {
                        // Reduce the amount for this repayment.
                        nextRepayment.PendingAmount = nextRepayment.PendingAmount - extraAmount;
                        nextRepayment.PaidAmount = extraAmount;
                        await _repaymentRepository.UpdateRepaymentAmount(nextRepayment);
                        extraAmount = 0;
                    }
                    else
                    {
                        // Settle the next repayment and reduce extra amount for subsequent repayments
                        await _repaymentRepository.SettleRepayment(nextRepayment.Id);
                        extraAmount = extraAmount - nextRepayment.PaidAmount;
                        if (extraAmount == 0 && repaymentIndex == activeRepayments.Count)
                        {
                            // In case all the repayments are settled, Settle the loan
                            await _loanRepository.SettleLoan(loan.Id);
                            _cache.Remove(loan.Id.ToString());
                            return true;
                        }
                    }
                }
            }

            return true;
        }


    }
}
