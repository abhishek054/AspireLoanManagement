using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Business;
using AspireLoanManagement.Utility.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AspireLoanManagement.Controllers
{
    public class RepaymentController : ControllerBase
    {
        private readonly ILoanService _loanService;
        public RepaymentController(ILoanService service)
        {
            _loanService = service;
        }

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

            return await _loanService.SettleRepayment(repayment);
        }

    }
}
