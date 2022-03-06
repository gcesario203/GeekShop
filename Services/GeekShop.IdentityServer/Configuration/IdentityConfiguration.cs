using Duende.IdentityServer.Models;

namespace GeekShop.IdentityServer.Configurations
{
    public static class IdentityConfiguration
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

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
                }
            };
    }
}