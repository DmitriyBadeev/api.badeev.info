using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Portfolio.Identity
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources(string apiPortfolio, string apiFinance)
        {
            return new List<ApiResource>
            {
                new ApiResource(apiPortfolio, "Тайные знания"),
                new ApiResource(apiFinance, "Черная бухгалтерия")
            };
        }

        public static IEnumerable<Client> GetSpaClient(string clientId, List<string> redirects, string apiPortfolio, string apiFinance)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = clientId,
                    ClientName = "Тайный бункер",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 3600 * 5,
                    RequireConsent = false,
                    AllowedCorsOrigins = 
                    { 
                        "http://localhost:3000", 
                        "http://localhost:3001", 
                        "https://badeev.info",
                        "https://cabinet.badeev.info"
                    },
                    PostLogoutRedirectUris = new List<string> 
                    {
                        "https://cabinet.badeev.info/signout",
                        "http://localhost:3000/signout"
                    },
                    RedirectUris = redirects,
                    AllowedScopes = 
                    { 
                        IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile,
                        apiPortfolio,
                        apiFinance
                    }
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
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Name, "Дмитрий"),
                        new Claim(JwtClaimTypes.FamilyName, "Бадеев"),
                        new Claim(JwtClaimTypes.Email, "mail@badeev.info"),
                        new Claim(JwtClaimTypes.Role, "Создатель"),
                        new Claim(JwtClaimTypes.Picture, "https://storage.badeev.info/avatar-min.png")
                    }
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
