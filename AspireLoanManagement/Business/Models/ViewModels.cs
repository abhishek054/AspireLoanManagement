using AspireLoanManagement.Repository;
using AspireLoanManagement.Utility.CommonEntities;

namespace AspireLoanManagement.Business.Models
{
    public class LoanModelVM
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int Term { get; set; }
        public LoanStatus Status { get; set; }
    }

    public class RepaymentModelVM
    {
        public int Id { get; set; }
        public decimal RepaymentAmount { get; set; }
    }
}
