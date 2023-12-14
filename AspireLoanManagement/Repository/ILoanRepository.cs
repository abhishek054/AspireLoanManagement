using AspireLoanManagement.Business.Models;

namespace AspireLoanManagement.Repository
{
    public interface ILoanRepository
    {
        Task<LoanModelDTO> GetLoanByIdAsync(int loanId);
        Task<int> AddLoanAsync(LoanModelVM loan);
        Task AddMultipleRepaymentAsync(List<RepaymentModelVM> repaymentList);
        Task ApproveLoan(int loanID);
        Task SettleRepayment(int repaymentId);
        Task SettleLoan(int loanID);
        Task UpdateRepaymentAmount(RepaymentModelDTO repayment);
    }
}
