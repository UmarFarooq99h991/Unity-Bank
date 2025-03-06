using Microsoft.AspNetCore.Identity;

namespace Unity_Bank.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
