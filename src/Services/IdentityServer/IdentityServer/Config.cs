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
                    ClientId="TestClient",
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    ClientSecrets={new Secret("testSecret".Sha256())},
                    AllowedScopes={ "None" }
                }
            };

        //定義Client可使用的Scope
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("None", "None"),
                new ApiScope("MovieAPI", "Movie API", userClaims: new[] { "role" })
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                //new ApiResource("roles", "Your role(s)", new[] { "role" })
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
                        new Claim(JwtClaimTypes.GivenName, "Mehmet"),
                        new Claim(JwtClaimTypes.FamilyName, "Demirci")
                    }
                }
            };
    }
}
