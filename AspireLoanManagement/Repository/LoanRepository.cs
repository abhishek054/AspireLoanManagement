
namespace AspireLoanManagement.Repository
{
    public class LoanRepository : ILoanRepository
    {
        public LoanRepository() { }

        public Task AddLoanAsync(LoanModelDTO loan)
        {
            throw new NotImplementedException();
        }

        public Task<LoanModelDTO> GetLoanByIdAsync(int loanId)
        {
            var loanDetails = new LoanModelDTO()
            {
                LoanAmount = 52,
                Id = loanId,
                RequestDate = DateTime.Now,
                Status = Utility.CommonEntities.LoanStatus.Pending,
                Term = 2
            };

            return Task.FromResult(loanDetails);
        }

        public Task<IEnumerable<LoanModelDTO>> GetLoansByCustomerIdAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLoanAsync(LoanModelDTO loan)
        {
            throw new NotImplementedException();
        }
    }
}
