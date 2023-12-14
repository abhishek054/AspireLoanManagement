using AspireLoanManagement.Business;
using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Utility.CommonEntities;
using AspireLoanManagement.Utility.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AspireLoanManagement.Controllers
{
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly IValidator<LoanModelVM> _loanValidator;
        private readonly IValidator<RepaymentModelVM> _repaymentValidator;
        public LoanController(ILoanService service, IValidator<LoanModelVM> loanValidator, IValidator<RepaymentModelVM> repaymentValidator)
        {
            _loanService = service;
            _loanValidator = loanValidator;
            _repaymentValidator = repaymentValidator;
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
            await _loanValidator.ValidateAndThrowAsync(loan);

            return await _loanService.AddLoanAsync(loan);
        }

        [HttpPost]
        [Route("api/Loan/SettleRepayment")]
        public async Task<bool> SettleRepayment([FromBody] RepaymentModelVM repayment)
        {
            await _repaymentValidator.ValidateAndThrowAsync(repayment);

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
