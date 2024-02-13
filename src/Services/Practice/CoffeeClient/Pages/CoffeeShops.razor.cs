
using CoffeeClient.Model;
using CoffeeClient.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;
namespace CoffeeClient.Pages
{
    public partial class CoffeeShops
    {
		private List<CoffeeShopModel> Shops = new();
		[Inject] private HttpClient HttpClient { get; set; }
		[Inject] private IConfiguration Config { get; set; }
		[Inject] private ITokenService TokenService { get; set; }

        public async Task LogTokenAndClaims()
        {
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            Debug.WriteLine($"Identity token: {identityToken}");

            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }

        protected override async Task OnInitializedAsync()
		{
			var tokenResponse = await TokenService.GetToken("CoffeeAPI.read");
			HttpClient.SetBearerToken(tokenResponse.AccessToken);

			var result = await HttpClient.GetAsync(Config["apiUrl"] + "/api/CoffeeShop");

			if (result.IsSuccessStatusCode)
			{
				Shops = await result.Content.ReadFromJsonAsync<List<CoffeeShopModel>>();
			}
		}
	}
}
