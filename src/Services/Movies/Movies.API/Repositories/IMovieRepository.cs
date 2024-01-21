using Movies.API.Model;

namespace Movies.API.Repositories
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAllMovies();

        Task<List<Movie>> GetMovieByOwner(string owner);

        Task<bool> CreateMovie(Movie movie);
        Task<bool> UpdateMovie(Movie movie);
        Task<bool> DeleteMovie(string title);
    }
}
