using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityServer4;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public class Config
    {
        //定義OAuth Client資訊
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId="MovieClient",
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    ClientSecrets={new Secret("testSecret".Sha256())},
                    AllowedScopes={ "MovieAPI" }
                },
                new Client
                {
                    ClientId = "movies_mvc_client",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("clientsecret".Sha256())
                    },
                    ClientName = "Movies MVC Web App",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequirePkce = false,
                    AllowRememberConsent=true,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:5002/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:5002/signout-callback-oidc"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.Email,
                        "roles",
                        "MovieAPI"
                    }
                }
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResource("roles","Your role(s)", new List<string>(){ "role" })
            };

        //定義Client可使用的Scope
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("None", "None"),
                new ApiScope("MovieAPI", "Movie API")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                //new ApiResource("roles", "Your role(s)", new[] { "role" })
            };

        public static List<TestUser> TestUsers =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "61029AAE-DACB-42FE-A466-3B045B6AFF51",
                    Username = "bies",
                    Password = "bies",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.GivenName, "bies"),
                        new Claim(JwtClaimTypes.FamilyName, "wwwe"),
                        new Claim(JwtClaimTypes.Email, "bies@aaa.con"),
                        new Claim(JwtClaimTypes.Address, "aaaaaaaaaaaa")
                    }
                }, 
                new TestUser
                {
                    SubjectId = "5555AAE-DACB-42FE-A466-3B045B6AFF51",
                    Username = "sean",
                    Password = "sean",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.GivenName, "sean"),
                        new Claim(JwtClaimTypes.FamilyName, "chu"),
                        new Claim(JwtClaimTypes.Email, "sean@aaa.con"),
                        new Claim(JwtClaimTypes.Address, "6666")
                    }
                }
            };
    }
}
