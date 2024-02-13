using CoffeeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeShopController : ControllerBase
    {
        private readonly ICoffeeShopService coffeeShopService;

        public CoffeeShopController(ICoffeeShopService coffeeShopService)
        {
            this.coffeeShopService = coffeeShopService;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var coffeeShops = await coffeeShopService.List();
            return Ok(coffeeShops);
        }
    }
}
