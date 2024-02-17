using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserTokenController : ControllerBase
    {

        private readonly ILogger<UserTokenController> _logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserTokenController(ILogger<UserTokenController> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        [HttpGet(Name = "GetUserInfo")]
        public async Task<UserInfoViewModel> Get()
        {
            var idpClient = httpClientFactory.CreateClient("IDPClient");

            var metaDataResponse = await idpClient.GetDiscoveryDocumentAsync();
            if (metaDataResponse.IsError)
            {
                throw new HttpRequestException("Something went wrong while requesting the discovery document");
            }

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            var userInfoResponse = await idpClient.GetUserInfoAsync(new UserInfoRequest
            {
                Address = metaDataResponse.UserInfoEndpoint,
                Token = accessToken
            });
            if (userInfoResponse.IsError)
            {
                throw new HttpRequestException("Something went wrong while requesting the user info");
            }

            var userInfoDictionary = new Dictionary<string, string>();
            foreach (var claim in userInfoResponse.Claims)
            {
                userInfoDictionary.Add(claim.Type, claim.Value);
            }
            var dict = userInfoResponse.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
            dict["accessToken"] = accessToken;
            dict["refreshToken"] = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            dict["id_token"] = await httpContextAccessor.HttpContext?.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            return new(dict);
        }

        public class UserInfoViewModel
        {
            public Dictionary<string, string> UserInfoDictionary { get; private set; }

            public UserInfoViewModel(Dictionary<string, string> userInfoDictionary)
            {
                UserInfoDictionary = userInfoDictionary ?? throw new ArgumentNullException(nameof(userInfoDictionary));
            }

        }
    }
}