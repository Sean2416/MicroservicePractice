using IdentityModel.Client;

namespace CoffeeClient.Services
{
	public interface ITokenService
	{
		Task<TokenResponse> GetToken(string scope);
	}
}
