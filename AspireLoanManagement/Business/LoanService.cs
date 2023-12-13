using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Repository;
using AspireLoanManagement.Utility.Cache;
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

        public Task AddLoanAsync(LoanModelVM loan)
        {
            throw new NotImplementedException();
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

        public Task<IEnumerable<LoanModelVM>> GetLoansByCustomerIdAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLoanAsync(LoanModelVM loan)
        {
            throw new NotImplementedException();
        }
    }
}
