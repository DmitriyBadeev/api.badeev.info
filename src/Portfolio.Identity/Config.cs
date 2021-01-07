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

        public static IEnumerable<Client> GetSpaClient(string commonClientId, string financeClientId, 
            List<string> redirects, List<string> financeRedirects, string apiPortfolio, string apiFinance)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = commonClientId,
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
                        "https://cabinet.badeev.info",
                        "https://investin.badeev.info"
                    },
                    PostLogoutRedirectUris = new List<string> 
                    {
                        "https://cabinet.badeev.info/signout",
                        "https://investin.badeev.info/signout",
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
                },
                new Client
                {
                    ClientId = financeClientId,
                    ClientName = "InvestIn",
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
                        "https://investin.badeev.info"
                    },
                    PostLogoutRedirectUris = new List<string> 
                    {
                        "https://investin.badeev.info/signout",
                        "http://localhost:3000/signout"
                    },
                    RedirectUris = financeRedirects,
                    AllowedScopes = 
                    { 
                        IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServerConstants.StandardScopes.Profile,
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
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "test@mail.com",
                    Password = "test",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Name, "Тестовый"),
                        new Claim(JwtClaimTypes.FamilyName, "пользователь"),
                        new Claim(JwtClaimTypes.Email, "test@mail.com"),
                        new Claim(JwtClaimTypes.Role, "Тестовый пользователь InvestIn"),
                        new Claim(JwtClaimTypes.Picture, "https://storage.badeev.info/avatar.png")
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
