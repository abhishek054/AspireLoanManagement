using System.Security.Claims;

namespace AspireLoanManagement.Business.Authentication
{
    public interface IAuthService
    {
        string GenerateToken(string username, List<Claim> claims);
    }
}
