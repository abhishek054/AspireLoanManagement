using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Repository;
using AspireLoanManagement.Utility.CommonEntities;

namespace AspireLoanManagement.Business
{
    public interface ILoanService
    {
        Task<LoanModelVM> GetLoanByIdAsync(int loanId);
        Task<LoanModelVM> AddLoanAsync(LoanModelVM loan);
        Task<bool> SettleRepayment(RepaymentModelVM repayment);
        Task<LoanStatus> ApproveLoan(int loanId);
    }
}
