using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Repository.Loan;
using AspireLoanManagement.Repository.Repayment;
using AspireLoanManagement.Utility.Cache;
using AspireLoanManagement.Utility.CommonEntities;
using AspireLoanManagement.Utility.Logger;
using AutoMapper;

namespace AspireLoanManagement.Business.Loan
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IRepaymentRepository _repaymentRepository;
        private readonly IMapper _mapper;
        private readonly IAspireLogger _logger;
        private readonly IAspireCacheService _cache;
        public LoanService(ILoanRepository loanRepository, IRepaymentRepository repaymentRepository, IMapper mapper, IAspireLogger logger, IAspireCacheService cache)
        {
            _loanRepository = loanRepository;
            _repaymentRepository = repaymentRepository;
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
            for (int i = 1; i <= loan.Term; i++)
            {
                repaymentModels.Add(new RepaymentModelVM()
                {
                    RepaymentAmount = loan.Amount / loan.Term,
                    UserId = loan.UserId,
                    LoanID = loanDto.Id,
                    ExpectedRepaymentDate = DateTime.UtcNow.AddDays(7 * i)
                });
            }

            await _repaymentRepository.AddMultipleRepaymentAsync(repaymentModels);

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

        public async Task<bool> IsLoanOwnedByUser(int userId, int loanId)
        {
            var loan = await _loanRepository.GetLoanByIdAsync(loanId);
            if (loan != null)
            {
                return loan.UserId == userId;
            }
            return false;
        }
    }
}
