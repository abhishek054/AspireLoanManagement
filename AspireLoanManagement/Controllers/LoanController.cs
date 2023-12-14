using AspireLoanManagement.Business;
using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Utility.CommonEntities;
using AspireLoanManagement.Utility.Validators;
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
            if(!validator.Validate(Id).IsValid)
            {
                throw new Exception("Invalid payload");
            }
            return await _loanService.GetLoanByIdAsync(Id);
        }

        [HttpPost]
        [Route("api/Loan/CreateLoan")]
        public async Task<LoanModelVM> CreateLoan([FromBody]LoanModelVM loan)
        {
            var validator = new CreateLoanPayloadValidator();
            if(!validator.Validate(loan).IsValid)
            {
                throw new Exception("Invalid payload");
            }
            return await _loanService.AddLoanAsync(loan);
        }

        [HttpPost]
        [Route("api/Loan/SettleRepayment")]
        public async Task<bool> SettleRepayment([FromBody]RepaymentModelVM repayment)
        {
            var validator = new RepaymentModelValidator();
            if (!validator.Validate(repayment).IsValid)
            {
                throw new Exception("Invalid payload");
            }
            return await _loanService.SettleRepayment(repayment);
        }

        [HttpPut]
        [Route("api/Loan/ApproveLoan/{Id}")]
        public async Task<LoanStatus> ApproveLoan(int Id)
        {
            return await _loanService.ApproveLoan(Id);
        }

    }
}
