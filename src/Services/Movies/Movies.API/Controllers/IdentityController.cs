using Movies.API.Model;
using Movies.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Movies.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IMovieRepository _repository;

        public IdentityController(IMovieRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value});
        }
    }
}
