using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Repository;
using AutoMapper;

namespace AspireLoanManagement.Business
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        public LoanService(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public Task AddLoanAsync(LoanModelVM loan)
        {
            throw new NotImplementedException();
        }

        public async Task<LoanModelVM> GetLoanByIdAsync(int loanId)
        {
            var loanInfoDb = await _loanRepository.GetLoanByIdAsync(loanId);
            return _mapper.Map<LoanModelVM>(loanInfoDb);

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
