using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Repository;

namespace AspireLoanManagement.Business
{
    public interface ILoanService
    {
        Task<LoanModelVM> GetLoanByIdAsync(int loanId);
        Task<IEnumerable<LoanModelVM>> GetLoansByCustomerIdAsync(int customerId);
        Task AddLoanAsync(LoanModelVM loan);
        Task UpdateLoanAsync(LoanModelVM loan);
    }
}
