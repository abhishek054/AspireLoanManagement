using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Utility.Validators;
using Microsoft.AspNetCore.Mvc;
using AspireLoanManagement.Business.Loan;
using Microsoft.AspNetCore.Authorization;
using AspireLoanManagement.Business.Repayment;

namespace AspireLoanManagement.Controllers
{
    public class RepaymentController : ControllerBase
    {
        private readonly IRepaymentService _repaymentService;
        public RepaymentController(IRepaymentService repaymentService)
        {
            _repaymentService = repaymentService;            
        }

        [Authorize(Policy = "LoanCustomerPolicy")]
        [HttpPost]
        [Route("api/Loan/SettleRepayment")]
        public async Task<bool> SettleRepayment([FromBody] RepaymentModelVM repayment)
        {
            var validator = new RepaymentModelValidator();
            var valid = await validator.ValidateAsync(repayment);
            if (!valid.IsValid)
            {
                throw new Exception("Error Occured");
            }

            return await _repaymentService.SettleRepayment(repayment);
        }

    }
}
