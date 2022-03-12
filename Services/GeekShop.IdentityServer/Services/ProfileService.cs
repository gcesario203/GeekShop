using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using GeekShop.IdentityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace GeekShop.IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsFactory;

        public ProfileService(UserManager<ApplicationUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IUserClaimsPrincipalFactory<ApplicationUser> userClaimsFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userClaimsFactory = userClaimsFactory;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var id = context.Subject.GetSubjectId();

            var user = await _userManager.FindByIdAsync(id);

            var userClaims = _userClaimsFactory.CreateAsync(user)
                            .GetAwaiter()
                            .GetResult()
                            .Claims
                            .ToList();

            userClaims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            userClaims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));

            if (_userManager.SupportsUserRole)
            {
                _userManager.GetRolesAsync(user)
                                        .GetAwaiter()
                                        .GetResult()
                                        .ToList()
                                        .ForEach(async x =>
                                        {
                                            userClaims.Add(new Claim(JwtClaimTypes.Role, x));

                                            if (_roleManager.SupportsRoleClaims)
                                            {
                                                var identityRole = await _roleManager.FindByNameAsync(x);
                                                if (identityRole != null) userClaims.AddRange(await _roleManager.GetClaimsAsync(identityRole));
                                            }
                                        });
            }

            context.IssuedClaims = userClaims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var id = context.Subject.GetSubjectId();

            var user = await _userManager.FindByIdAsync(id);

            context.IsActive = user != null;
        }
    }
}