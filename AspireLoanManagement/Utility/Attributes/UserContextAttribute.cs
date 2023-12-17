using AspireLoanManagement.Business.Loan;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspireLoanManagement.Utility.Attributes
{
    public class UserContextAttribute : Attribute, IAuthorizationFilter
    {
        public UserContextAttribute()
        {
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User?.Identity?.Name;

            if (userId == default)
            {
                context.Result = new ForbidResult();
                return;
            }

            context.HttpContext.Items["userId"] = userId;
        }
    }
}
