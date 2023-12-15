using AspireLoanManagement.Controllers;

namespace AspireLoanManagement.Business.Models.User
{
    public class AspireUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
        public UserRole UserRole { get; set; }
    }
}
