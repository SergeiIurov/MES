using Microsoft.AspNetCore.Identity;

namespace ControlBoard.Web.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; }
        public string MachineName { get; set; }
        public string ActiveUserName { get; set; }
    }
}
