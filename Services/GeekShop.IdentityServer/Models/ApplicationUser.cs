using Microsoft.AspNetCore.Identity;

namespace GeekShop.IdentityServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}