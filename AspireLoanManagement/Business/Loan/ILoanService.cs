using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Repository;
using AspireLoanManagement.Utility.CommonEntities;

namespace AspireLoanManagement.Business.Loan
{
    public interface ILoanService
    {
        Task<LoanModelVM> GetLoanByIdAsync(int loanId);
        Task<LoanModelVM> AddLoanAsync(LoanModelVM loan);
        Task<LoanStatus> ApproveLoan(int loanId);
        Task<bool> IsLoanOwnedByUser(int userName, int loanId);
    }
}
