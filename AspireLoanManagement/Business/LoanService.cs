using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Repository;
using AspireLoanManagement.Utility.Cache;
using AspireLoanManagement.Utility.CommonEntities;
using AspireLoanManagement.Utility.Logger;
using AutoMapper;

namespace AspireLoanManagement.Business
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly IAspireLogger _logger;
        private readonly IAspireCacheService _cache;
        public LoanService(ILoanRepository loanRepository, IMapper mapper, IAspireLogger logger, IAspireCacheService cache)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
        }

        public async Task<LoanModelVM> AddLoanAsync(LoanModelVM loan)
        {
            _logger.Log(LogLevel.Info, $"User: {loan.UserId} applied for a loan of amount: {loan.Amount}, term: {loan.Term}");
            
            var loanDto = await _loanRepository.AddLoanAsync(loan);

            _logger.Log(LogLevel.Info, $"Loan created with loanID: {loanDto.Id}");

            var loanVM = _mapper.Map<LoanModelVM>(loanDto);

            List<RepaymentModelVM> repaymentModels = new List<RepaymentModelVM>() { };
            for(int i=1; i<= loan.Term; i++)
            {
                repaymentModels.Add(new RepaymentModelVM()
                {
                    RepaymentAmount = loan.Amount / loan.Term,
                    UserId = loan.UserId,
                    LoanID = loanDto.Id,
                    ExpectedRepaymentDate = DateTime.UtcNow.AddDays(7 * i)
                });
            }

            await _loanRepository.AddMultipleRepaymentAsync(repaymentModels);

            loanVM.Repayments = repaymentModels;

            return loanVM;
        }

        public async Task<LoanStatus> ApproveLoan(int loanId)
        {
            _logger.Log(LogLevel.Info, $"Approving loan for loanId: {loanId}");
            var status = await _loanRepository.ApproveLoan(loanId);
            // Expire the loan cache in case of update
            _cache.Remove(loanId.ToString());
            return status;
        }

        public async Task<LoanModelVM> GetLoanByIdAsync(int loanId)
        {
            _logger.Log(LogLevel.Info, $"Retrieve loan details for loanId: {loanId}");
            var loanData = _cache.Get<LoanModelVM>(loanId.ToString());
            if (loanData != null)
            {
                return loanData;
            }
            var loanInfoDb = await _loanRepository.GetLoanByIdAsync(loanId);
            var loanInfoVM = _mapper.Map<LoanModelVM>(loanInfoDb);

            _cache.Set(loanId.ToString(), loanInfoVM);
            
            return loanInfoVM;

        }

        public async Task<bool> SettleRepayment(RepaymentModelVM repayment)
        {
            // Get the loan details from DB
            var loan = await _loanRepository.GetLoanByIdAsync(repayment.LoanID);

            if(loan.UserId != repayment.UserId)
            {
                throw new Exception("Invalid settlement");
            }

            if(loan.Status != LoanStatus.Approved)
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
            if(repayment.RepaymentAmount > pendingLoanAmount)
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
            await _loanRepository.SettleRepayment(latestActiveRepayment.Id);

            // If the settled repayment was the last active repayment of the loan, settle the Loan and exit
            if(activeRepayments.Count == 1)
            {
                await _loanRepository.SettleLoan(loan.Id);
                _cache.Remove(loan.Id.ToString());
                return true;
            }

            // Update the next Repayments if necessary
            if(repayment.RepaymentAmount > repaymentAmount)
            {
                // Tracking other active repayments in order of expected payment date
                activeRepayments = activeRepayments.Where(x => x.Id != latestActiveRepayment.Id).OrderBy(x => x.ExpectedRepaymentDate).ToList();
                decimal extraAmount = repayment.RepaymentAmount - repaymentAmount;
                int repaymentIndex = 0; // To track other active repayments based on index

                while(extraAmount > 0 && repaymentIndex < activeRepayments.Count)
                {
                    var nextRepayment = activeRepayments[repaymentIndex++];
                    if (extraAmount < nextRepayment.PendingAmount)
                    {
                        // Reduce the amount for this repayment.
                        nextRepayment.PendingAmount = nextRepayment.PendingAmount - extraAmount;
                        nextRepayment.PaidAmount = extraAmount;
                        await _loanRepository.UpdateRepaymentAmount(nextRepayment);
                        extraAmount = 0;
                    }
                    else
                    {
                        // Settle the next repayment and reduce extra amount for subsequent repayments
                        await _loanRepository.SettleRepayment(nextRepayment.Id);
                        extraAmount = extraAmount - nextRepayment.PaidAmount;
                        if(extraAmount == 0 && repaymentIndex == activeRepayments.Count)
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
