using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Portfolio.Identity
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources(string api)
        {
            return new List<ApiResource>
            {
                new ApiResource(api, "Тайные знания")
            };
        }

        public static IEnumerable<Client> GetSpaClient(string clientId, List<string> redirects, string api)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = clientId,
                    ClientName = "Тайный бункер",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false,
                    AllowedCorsOrigins = 
                    { 
                        "http://localhost:3000", 
                        "http://localhost:3001", 
                        "https://badeev.info",
                        "https://cabinet.badeev.info"
                    },
                    RedirectUris = redirects,
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, api }
                }
            };
        }

        public static List<TestUser> GetUser(string login, string password)
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = login,
                    Password = password,
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }
}
