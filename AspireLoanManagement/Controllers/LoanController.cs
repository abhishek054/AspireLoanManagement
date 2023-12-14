using AspireLoanManagement.Business;
using AspireLoanManagement.Business.Models;
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
            return await _loanService.GetLoanByIdAsync(Id);
        }

        [HttpPost]
        [Route("api/Loan/CreateLoan")]
        public async Task<LoanModelVM> CreateLoan([FromBody]LoanModelVM loan)
        {
            return await _loanService.AddLoanAsync(loan);
        }

    }
}
