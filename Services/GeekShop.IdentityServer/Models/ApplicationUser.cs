using Microsoft.AspNetCore.Identity;

namespace GeekShop.IdentityServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        private string FirstName { get; set; }
        private string LastName { get; set; }
    }
}