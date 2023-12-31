﻿using AspireLoanManagement.Utility.CommonEntities;

namespace AspireLoanManagement.Repository
{
    public class LoanModelDTO
    {
        public LoanModelDTO()
        {
            Repayments = new List<RepaymentModelDTO>() { };
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal LoanAmount { get; set; }
        public int Term { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? SettledDate { get; set; }
        public LoanStatus Status { get; set; }
        public ICollection<RepaymentModelDTO> Repayments { get; set; }
    }

    public class RepaymentModelDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LoanID { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal PaidAmount { get; set; } = 0;
        public DateTime ExpectedRepaymentDate {  get; set; }
        public DateTime? ActualRepaymentDate { get; set; }
        public RepaymentStatus Status { get; set; }
        public virtual LoanModelDTO LoanModelDTO { get; set; }
    }
}
