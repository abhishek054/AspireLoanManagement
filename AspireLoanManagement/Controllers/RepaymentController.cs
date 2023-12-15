using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Utility.Validators;
using Microsoft.AspNetCore.Mvc;
using AspireLoanManagement.Business.Loan;
using Microsoft.AspNetCore.Authorization;

namespace AspireLoanManagement.Controllers
{
    public class RepaymentController : ControllerBase
    {
        private readonly ILoanService _loanService;
        public RepaymentController(ILoanService service)
        {
            _loanService = service;
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

            return await _loanService.SettleRepayment(repayment);
        }

    }
}
