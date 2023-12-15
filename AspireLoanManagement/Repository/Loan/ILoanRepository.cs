using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Utility.CommonEntities;

namespace AspireLoanManagement.Repository.Loan
{
    public interface ILoanRepository
    {
        Task<LoanModelDTO> GetLoanByIdAsync(int loanId);
        Task<LoanModelDTO> AddLoanAsync(LoanModelVM loan);
        Task<LoanStatus> ApproveLoan(int loanID);
        Task SettleLoan(int loanID);
    }
}
