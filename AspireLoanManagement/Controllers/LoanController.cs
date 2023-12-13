using AspireLoanManagement.Business;
using AspireLoanManagement.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspireLoanManagement.Controllers
{
    public class LoanController : ControllerBase
    {
        private readonly ILoanService loanService;
        public LoanController(ILoanService service)
        {
            loanService = service;
        }

        [HttpGet]
        [Route("api/Loan/GetLoanById/{Id}")]
        public async Task<LoanModelVM> GetLoanByID(int Id)
        {
            return await loanService.GetLoanByIdAsync(Id);
        }

    }
}
