using CoffeeAPI.Model;

namespace CoffeeAPI.Services
{
    public interface ICoffeeShopService
    {
        Task<List<CoffeeShopModel>> List();
    }
}
