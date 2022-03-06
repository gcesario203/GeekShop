using System.Security.Claims;
using GeekShop.IdentityServer.Configurations;
using GeekShop.IdentityServer.Initializer.Interfaces;
using GeekShop.IdentityServer.Models;
using GeekShop.IdentityServer.Models.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace GeekShop.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly MySqlContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;
        public DbInitializer(MySqlContext context, UserManager<ApplicationUser> user, RoleManager<IdentityRole> role)
        {
            _context = context;
            _user = user;
            _role = role;           
        }
        public void Initialize()
        {
            if(_role.FindByNameAsync(IdentityConfiguration.Admin).Result != null) return;

            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult();
            
            var admin = new ApplicationUser()
            {
                UserName = "CesaoAdmin",
                Email = "cesao@desenv.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 (55) 12345-6789",
                FirstName="Gabriel",
                LastName = "Admin"
            };

            _user.CreateAsync(admin, "Teste123$").GetAwaiter().GetResult();

            _user.AddToRoleAsync(admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();

            var adminClaims = _user.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} - {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin),
            }).Result;

            var client = new ApplicationUser()
            {
                UserName = "Cesaoclient",
                Email = "cesao@desenvV2.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 (55) 12345-6719",
                FirstName="Gabriel",
                LastName = "client"
            };

            _user.CreateAsync(client, "Teste123$").GetAwaiter().GetResult();

            _user.AddToRoleAsync(client, IdentityConfiguration.Client).GetAwaiter().GetResult();

            var clientClaims = _user.AddClaimsAsync(client, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{client.FirstName} - {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, client.FirstName),
                new Claim(JwtClaimTypes.FamilyName, client.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client),
            }).Result;

        }
    }
}