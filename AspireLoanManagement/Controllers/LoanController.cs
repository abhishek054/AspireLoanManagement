using AspireLoanManagement.Business.Loan;
using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Utility.Attributes;
using AspireLoanManagement.Utility.CommonEntities;
using AspireLoanManagement.Utility.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspireLoanManagement.Controllers
{
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
        [Route("api/Loan/GetLoanById/{Id}")]
        public async Task<LoanModelVM> GetLoanByID(int Id)
        {
            var validator = new GetLoanPayloadValidator();
            if (!validator.Validate(Id).IsValid)
            {
                throw new Exception("Invalid payload");
            }

            if (HttpContext.Items.TryGetValue("userId", out var userId))
            {
                if (!await _loanService.IsLoanOwnedByUser(Convert.ToInt32(userId), Id))
                {
                    throw new InvalidOperationException("Loan do not belong to this user");
                }

                return await _loanService.GetLoanByIdAsync(Id);
            }

            throw new Exception("User information missing from request context");
        }

        [Authorize(Policy = "LoanCustomerPolicy")]
        [HttpPost]
        [Route("api/Loan/CreateLoan")]
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
        [Route("api/Loan/ApproveLoan/{Id}")]
        public async Task<LoanStatus> ApproveLoan(int Id)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Unauthorized user");
            }
            return await _loanService.ApproveLoan(Id);
        }

    }
}
