
using CoffeeClient.Model;
using CoffeeClient.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;


namespace CoffeeClient.Pages
{
	public partial class CoffeeShops
	{
		private List<CoffeeShopModel> Shops = new();
		[Inject] private HttpClient HttpClient { get; set; }
		[Inject] private IConfiguration Config { get; set; }
		[Inject] private ITokenService TokenService { get; set; }

        protected override async Task OnInitializedAsync()
        {

            var tokenResponse = await TokenService.GetToken("CoffeeAPI.read");
			HttpClient.SetBearerToken(tokenResponse.AccessToken);

			var result = await HttpClient.GetAsync(Config["apiUrl"] + "/coffeeapi/shop");

			if (result.IsSuccessStatusCode)
			{
				Shops = await result.Content.ReadFromJsonAsync<List<CoffeeShopModel>>();
			}
		}
	}
}
