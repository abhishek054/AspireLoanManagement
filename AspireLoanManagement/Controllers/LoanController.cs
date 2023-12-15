using AspireLoanManagement.Business.Loan;
using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Utility.Attributes;
using AspireLoanManagement.Utility.CommonEntities;
using AspireLoanManagement.Utility.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspireLoanManagement.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;
        public LoanController(ILoanService service)
        {
            _loanService = service;
        }

        [Authorize(Policy = "LoanCustomerPolicy")]
        [LoanToUserMapping]
        [HttpGet]
        [Route("api/v{version:apiVersion}/[controller]/GetLoanById/{loanId}")]
        public async Task<LoanModelVM> GetLoanByID(int loanId)
        {
            var validator = new GetLoanPayloadValidator();
            if (!validator.Validate(loanId).IsValid)
            {
                throw new Exception("Invalid payload");
            }

            if (HttpContext.Items.TryGetValue("userId", out var userId))
            {
                if (!await _loanService.IsLoanOwnedByUser(Convert.ToInt32(userId), loanId))
                {
                    throw new InvalidOperationException("Loan do not belong to this user");
                }

                return await _loanService.GetLoanByIdAsync(loanId);
            }

            throw new Exception("User information missing from request context");
        }

        [Authorize(Policy = "LoanCustomerPolicy")]
        [HttpPost]
        [Route("api/v{version:apiVersion}/[controller]/CreateLoan")]
        public async Task<LoanModelVM> CreateLoan([FromBody] LoanModelVM loan)
        {
            var validator = new CreateLoanPayloadValidator();
            var valid = await validator.ValidateAsync(loan);
            if (!valid.IsValid)
            {
                throw new Exception("Error Occured");
            }

            return await _loanService.AddLoanAsync(loan);
        }

        [Authorize(Policy = "LoanManagerPolicy")]
        [HttpPut]
        [Route("api/v{version:apiVersion}/[controller]/ApproveLoan/{loanId}")]
        public async Task<LoanStatus> ApproveLoan(int loanId)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Unauthorized user");
            }
            return await _loanService.ApproveLoan(loanId);
        }

    }
}
