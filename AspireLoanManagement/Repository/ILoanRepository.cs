namespace AspireLoanManagement.Repository
{
    public interface ILoanRepository
    {
        Task<LoanModelDTO> GetLoanByIdAsync(int loanId);
        Task<IEnumerable<LoanModelDTO>> GetLoansByCustomerIdAsync(int customerId);
        Task AddLoanAsync(LoanModelDTO loan);
        Task UpdateLoanAsync(LoanModelDTO loan);
    }
}
