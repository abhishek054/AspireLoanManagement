using AspireLoanManagement.Business;
using AspireLoanManagement.Business.Models;
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

        [HttpGet]
        [Route("api/Loan/GetLoanById/{Id}")]
        public async Task<LoanModelVM> GetLoanByID(int Id)
        {
            var validator = new GetLoanPayloadValidator();
            if (!validator.Validate(Id).IsValid)
            {
                throw new Exception("Invalid payload");
            }
            return await _loanService.GetLoanByIdAsync(Id);
        }

        [HttpPost]
        [Route("api/Loan/CreateLoan")]
        public async Task<LoanModelVM> CreateLoan([FromBody] LoanModelVM loan)
        {
            var validator = new CreateLoanPayloadValidator();
            var valid = await validator.ValidateAsync(loan);
            if(!valid.IsValid)
            {
                throw new Exception("Error Occured");
            }

            return await _loanService.AddLoanAsync(loan);
        }

        [Authorize(Roles = "LoanManager")]
        [HttpPut]
        [Route("api/Loan/ApproveLoan/{Id}")]
        public async Task<LoanStatus> ApproveLoan(int Id)
        {
            if(!ModelState.IsValid)
            {
                throw new Exception("Unauthorized user");
            }
            return await _loanService.ApproveLoan(Id);
        }

    }
}
