using AspireLoanManagement.Business.Models;

namespace AspireLoanManagement.Business.Repayment
{
    public interface IRepaymentService
    {
        Task<bool> SettleRepayment(RepaymentModelVM repayment);
    }
}
