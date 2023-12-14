
using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Utility.CommonEntities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspireLoanManagement.Repository
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LoanDbContext _context;
        private readonly IMapper _mapper;

        public LoanRepository(LoanDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<LoanModelDTO> AddLoanAsync(LoanModelVM loan)
        {
            var loanDto = _mapper.Map<LoanModelDTO>(loan);
            loanDto.RequestDate = DateTime.Now;
            loanDto.Status = LoanStatus.Pending;
            _context.Loans.Add(loanDto);
            await _context.SaveChangesAsync();
            return loanDto;
        }

        public async Task AddMultipleRepaymentAsync(List<RepaymentModelVM> repayments)
        {
            var repaymentDto = _mapper.Map<List<RepaymentModelDTO>>(repayments);
            _context.Repayments.AddRange(repaymentDto);
            await _context.SaveChangesAsync();
        }

        public async Task<LoanStatus> ApproveLoan(int loanID)
        {
            var loan = await _context.Loans.FindAsync(loanID);
            if (loan == null)
            {
                throw new InvalidDataException("No loan exists for mentioned loanID");
            }

            loan.Status = LoanStatus.Approved;

            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
            return loan.Status;
        }

        public async Task<LoanModelDTO> GetLoanByIdAsync(int loanId)
        {
            var loanDto = await _context.Loans.FirstOrDefaultAsync(x => x.Id == loanId);
            if (loanDto == null)
            {
                throw new InvalidDataException($"No oan available with ID: {loanId}");
            }
            loanDto.Repayments = await _context.Repayments.Where(x => x.LoanID ==  loanId).ToListAsync();
            return loanDto;
        }

        public async Task SettleRepayment(int repaymentId)
        {
            var rep = await _context.Repayments.FirstOrDefaultAsync(x => x.Id == repaymentId);
            if(rep == null)
            {
                throw new Exception($"No repayment available for repaymentID: {repaymentId}");
            }
            rep.ActualRepaymentDate = DateTime.UtcNow;
            rep.Status = RepaymentStatus.Paid;
            rep.PaidAmount += rep.PendingAmount;
            rep.PendingAmount = 0;
            _context.Repayments.Update(rep);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRepaymentAmount(RepaymentModelDTO repayment)
        {
            var rep = await _context.Repayments.FirstOrDefaultAsync(x => x.Id == repayment.Id);
            if (rep == null)
            {
                throw new Exception($"No repayment available for repaymentID: {repayment.Id}");
            }
            rep.PendingAmount = repayment.PendingAmount;
            _context.Repayments.Update(rep);
            await _context.SaveChangesAsync();
        }

        public async Task SettleLoan(int loanID)
        {
            var loan = await _context.Loans.FirstOrDefaultAsync(x => x.Id == loanID && x.Status != Utility.CommonEntities.LoanStatus.Paid);
            if(loan == null)
            {
                throw new Exception($"No active loan found with id: {loanID}");
            }
            loan.Status = Utility.CommonEntities.LoanStatus.Paid;
            loan.SettledDate = DateTime.UtcNow;
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
        }
    }
}
