using IdentityServer4.Models;

//using IdentityServer4.EntityFramework.Entities;

namespace IdentityApi.Configuration
{
    public static class Config
    {
        public static IEnumerable<Client> Clients => new Client[]
        {
            new Client
            {
                ClientId = "bo-web",
                AllowedGrantTypes = new List<String> { GrantType.ResourceOwnerPassword },
                ClientSecrets = { new Secret("secret".Sha256()) },
                RequireClientSecret = false,
                RequireConsent = false,
                RedirectUris = new List<string> { "http://localhost:8100" },
                PostLogoutRedirectUris = new List<string> { "http://localhost:8100/" },
                AllowedScopes = new List<string> { "api-gateway", "openid", "profile" },
                AllowedCorsOrigins = new List<string> { "http://localhost:8100", "http://localhost:8101" },
                AccessTokenLifetime = 86400,
                AllowAccessTokensViaBrowser = true,
                AllowOfflineAccess = true,
                RequirePkce = true,
            }
        };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource
            {
                Name = "api-gateway",
                DisplayName = "Api Gateway",
                Scopes = new List<string>
                {
                    "write",
                    "read"
                }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
            //new ApiScope("openid"),
            //new ApiScope("profile"),
            new ApiScope("api-gateway")
        };

        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
    }
}
