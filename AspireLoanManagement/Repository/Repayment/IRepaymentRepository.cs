using AspireLoanManagement.Business.Models;

namespace AspireLoanManagement.Repository.Repayment
{
    public interface IRepaymentRepository
    {
        Task AddMultipleRepaymentAsync(List<RepaymentModelVM> repaymentList);
        Task SettleRepayment(int repaymentId);
        Task UpdateRepaymentAmount(RepaymentModelDTO repayment);

    }
}
