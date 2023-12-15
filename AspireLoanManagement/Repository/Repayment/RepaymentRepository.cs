using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Utility.CommonEntities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AspireLoanManagement.Repository.Repayment
{
    public class RepaymentRepository: IRepaymentRepository
    {
        private readonly AspireDbContext _context;
        private readonly IMapper _mapper;

        public RepaymentRepository(AspireDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task SettleRepayment(int repaymentId)
        {
            var rep = await _context.Repayments.FirstOrDefaultAsync(x => x.Id == repaymentId);
            if (rep == null)
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

        public async Task AddMultipleRepaymentAsync(List<RepaymentModelVM> repayments)
        {
            var repaymentDto = _mapper.Map<List<RepaymentModelDTO>>(repayments);
            _context.Repayments.AddRange(repaymentDto);
            await _context.SaveChangesAsync();
        }
    }
}
