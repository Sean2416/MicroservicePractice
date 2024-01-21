using Movies.API.Model;
using Movies.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Movies.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _repository;

        public MoviesController(IMovieRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet(Name = "GetAllMovie")]
        [ProducesResponseType(typeof(Movie), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Movie>>> GetAllMovie()
        {

            var movies = await _repository.GetAllMovies();
            return Ok(movies);
        }

        [HttpGet("{owner}", Name = "GetMovie")]
        [ProducesResponseType(typeof(Movie), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Movie>> GetMovie(string owner)
        {
            var movie = await _repository.GetMovieByOwner(owner);
            return Ok(movie);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Movie), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Movie>> CreateMovie([FromBody] Movie movie)
        {
            return Ok(await _repository.CreateMovie(movie));
        }

        [HttpPut]
        [ProducesResponseType(typeof(Movie), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Movie>> UpdateMovie([FromBody] Movie movie)
        {
            return Ok(await _repository.UpdateMovie(movie));
        }

        [HttpDelete("{title}", Name = "DeleteMovie")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteMovie(string title)
        {
            return Ok(await _repository.DeleteMovie(title));
        }
    }
}
