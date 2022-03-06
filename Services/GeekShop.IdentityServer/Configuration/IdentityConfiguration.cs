using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace GeekShop.IdentityServer.Configurations
{
    public static class IdentityConfiguration
    {
        public const string Admin = "Admin";
        public const string Client = "Client";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("geek_shopping", "GeekShop Server"),
                new ApiScope(name: "read","Read data"),
                new ApiScope(name: "write","Write data"),
                new ApiScope(name: "delete", "Delete data"),
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = {
                        new Secret("SheGotTheEyeOfThePanther".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {
                        "read",
                        "write",
                        "profile"
                    }
                },
                new Client
                {
                    ClientId = "geek_shopping",
                    ClientSecrets = {
                        new Secret("SheGotTheEyeOfThePanther".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {
                        "http://localhost:5080/signin-oidc"
                    },
                    PostLogoutRedirectUris = {
                        "http://localhost:5080/signou-callback-oidc"
                    },
                    AllowedScopes = new List<string>{
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Profile,
                        "geek_shopping"
                    }
                },
            };
    }
}