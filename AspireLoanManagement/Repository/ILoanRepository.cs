using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Utility.CommonEntities;

namespace AspireLoanManagement.Repository
{
    public interface ILoanRepository
    {
        Task<LoanModelDTO> GetLoanByIdAsync(int loanId);
        Task<LoanModelDTO> AddLoanAsync(LoanModelVM loan);
        Task AddMultipleRepaymentAsync(List<RepaymentModelVM> repaymentList);
        Task<LoanStatus> ApproveLoan(int loanID);
        Task SettleRepayment(int repaymentId);
        Task SettleLoan(int loanID);
        Task UpdateRepaymentAmount(RepaymentModelDTO repayment);
    }
}
