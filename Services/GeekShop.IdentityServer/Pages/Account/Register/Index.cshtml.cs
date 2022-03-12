using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using GeekShop.IdentityServer.Models;
using GeekShop.IdentityServer.Pages.Login;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static GeekShop.IdentityServer.Pages.Login.ViewModel;

namespace GeekShop.IdentityServer.Pages.Account.Register
{
    public class Index : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _sigInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IIdentityProviderStore _identityProviderStore;
        public ViewModel View { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public Index(
    IIdentityServerInteractionService interaction,
    IClientStore clientStore,
    IAuthenticationSchemeProvider schemeProvider,
    IIdentityProviderStore identityProviderStore,
    IEventService events,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> sigInManager,
    RoleManager<IdentityRole> roleManager
    )
        {
            // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
            _userManager = userManager;
            _sigInManager = sigInManager;
            _roleManager = roleManager;

            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _identityProviderStore = identityProviderStore;
            _events = events;
        }

        public async Task<IActionResult> OnGet(string returnUrl)
        {
            // Input.ReturnUrl = returnUrl;
            await BuildModelAsync(returnUrl);
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (Input.Button == "login")
                return RedirectToPage("/Account/Login/Index", new { Input.ReturnUrl });

            var user = new ApplicationUser()
            {
                UserName = Input.UserName,
                Email = Input.Email,
                EmailConfirmed = true,
                FirstName = Input.FirstName,
                LastName = Input.LastName
            };


            var result = await _userManager.CreateAsync(user, Input.Password);

            if (!result.Succeeded)
                return Page();

            if (!await _roleManager.RoleExistsAsync(Input.RoleName))
            {
                var userRole = new IdentityRole
                {
                    Name = Input.RoleName,
                    NormalizedName = Input.RoleName
                };

                await _roleManager.CreateAsync(userRole);
            }

            await _userManager.AddToRoleAsync(user, Input.RoleName);

            await _userManager.AddClaimsAsync(user, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, Input.UserName),
                new Claim(JwtClaimTypes.Email, Input.Email),
                new Claim(JwtClaimTypes.FamilyName, Input.LastName),
                new Claim(JwtClaimTypes.GivenName, Input.FirstName),
                new Claim(JwtClaimTypes.Role, "User"),
            });

            var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);
            var loginResult = await _sigInManager.PasswordSignInAsync(Input.UserName, Input.Password, false, false);

            if (loginResult.Succeeded)
            {
                var checkUser = await _userManager.FindByNameAsync(Input.UserName);

                await _events.RaiseAsync(new UserLoginSuccessEvent(checkUser.UserName, checkUser.Id, checkUser.FirstName, checkUser.LastName));

                if (context == null)
                    return Redirect(Input.ReturnUrl);

                if (context.IsNativeClient())
                    return this.LoadingPage(Input.ReturnUrl);

                return Redirect(Input.ReturnUrl);
            }

            await _events.RaiseAsync(new UserLoginFailureEvent("Nobody", "Error creating user"));

            return Page();
        }
        private async Task BuildModelAsync(string returnUrl)
        {

            Input = new InputModel
            {
                ReturnUrl = returnUrl
            };

            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

                View = new ViewModel
                {
                    EnableLocalLogin = local
                };

                Input.UserName = context?.LoginHint;

                if (!local)
                {
                    View.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }
            }
        }
    }
}