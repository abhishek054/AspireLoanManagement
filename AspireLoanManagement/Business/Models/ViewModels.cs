using AspireLoanManagement.Repository;
using AspireLoanManagement.Utility.CommonEntities;

namespace AspireLoanManagement.Business.Models
{
    public class LoanModelVM
    {
        public int Id { get; set; }
        public required decimal Amount { get; set; }
        public required int Term { get; set; }
        public required int UserId { get; set; }
        public LoanStatus Status { get; set; }
        public List<RepaymentModelVM> Repayments { get; set; }
        public LoanModelVM()
        {
            Repayments = new List<RepaymentModelVM>() { };
        }
    }

    public class RepaymentModelVM
    {
        public int Id { get; set; }
        public decimal RepaymentAmount { get; set; }
        public required int UserId { get; set; }
        public int LoanID { get; set; }
        public DateTime ExpectedRepaymentDate { get; set; }
    }
}
