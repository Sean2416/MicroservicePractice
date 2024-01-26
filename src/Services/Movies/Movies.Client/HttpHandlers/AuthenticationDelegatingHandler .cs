using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Movies.Client.HttpHandlers
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        //private readonly IHttpClientFactory httpClientFactory;
        //private readonly ClientCredentialsTokenRequest clientCredentialsTokenRequest;

        //public AuthenticationDelegatingHandler(IHttpClientFactory httpClientFactory, ClientCredentialsTokenRequest clientCredentialsTokenRequest)
        //{
        //    this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        //    this.clientCredentialsTokenRequest = clientCredentialsTokenRequest ?? throw new ArgumentNullException(nameof(clientCredentialsTokenRequest));
        //}

        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <summary>
        /// 自行實作在request發送前，將AccessToken帶入Request
        /// 被內建AddUserAccessTokenHandler 取代
        /// </summary>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            /*實作從Auth Server進行Client Credential並取得Token
           
            var client = httpClientFactory.CreateClient("IDPClient");
            var clientCredentialsTokenRequest = new ClientCredentialsTokenRequest
            {
                Address = "https://localhost:5006/connect/token",
                ClientId = "MovieClient",
                ClientSecret = "testSecret",
                Scope = "MovieAPI"
            };

            //var token = await client.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);
            //if (token.IsError)
            //{
            //    throw new HttpRequestException("Something went wrong while requesting the access token");
            //}

            //request.SetBearerToken(token.AccessToken!);
            //return await base.SendAsync(request, cancellationToken);
            */

            string accessToken = (await httpContextAccessor.HttpContext?.GetTokenAsync(OpenIdConnectParameterNames.AccessToken)) ?? throw new ArgumentNullException("access token is null");

            request.SetBearerToken(accessToken);
            
            return await base.SendAsync(request, cancellationToken);
        }
    }
}