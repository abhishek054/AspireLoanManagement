using AspireLoanManagement.Utility.CommonEntities;

namespace AspireLoanManagement.Repository
{
    public class LoanModelDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int Term { get; set; }
        public DateTime RequestDate { get; set; }
        public LoanStatus Status { get; set; }
    }
}
