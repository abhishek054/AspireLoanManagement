using AspireLoanManagement.Business.Authentication;
using AspireLoanManagement.Business.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspireLoanManagement.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("api/v{version:apiVersion}/[controller]/Login")]
        public IActionResult Login([FromBody]AspireUser loginUser)
        {
            // Authenticate user using username and password
            // User Management will track the user and the roles assigned to them
            AspireUser user = new AspireUser()
            {
                UserRole = UserRole.LoanManager,
                JoiningDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(100)),
                UserId = 23,
                UserName = "Aspire test user"
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginUser.UserName),

                // Assign the proper claim as per the role assigned to user
                new Claim(ClaimTypes.Role, user.UserRole == UserRole.LoanManager ? "LoanManager" : "LoanCustomer")
            };

            var token = _authService.GenerateToken(loginUser.UserName, claims);

            return Ok(new { Token = token });
        }
    }
}
