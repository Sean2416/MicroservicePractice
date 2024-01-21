using Dapper;
using Microsoft.Data.SqlClient;
using Movies.API.Model;

namespace Movies.API.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IConfiguration _configuration;

        public MovieRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<List<Movie>> GetAllMovies()
        {
            using var connection = new SqlConnection
                         (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var result = await connection.QueryAsync<Movie>("SELECT * FROM Movie");

            return result.ToList();
        }


        public async Task<List<Movie>> GetMovieByOwner(string owner)
        {
            using var connection = new SqlConnection
                         (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var result = await connection.QueryAsync<Movie>
               ("SELECT * FROM Movie WHERE Owner = @Owner", new { Owner = owner });

            return result.ToList();
        }


        public async Task<bool> CreateMovie(Movie entity)
        {
            using var connection = new SqlConnection
                         (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =
                await connection.ExecuteAsync
                    ("INSERT INTO Movie (Id, Title, Owner, Genre,ReleaseDate) VALUES (@Id, @Title, @Owner, @Genre,@ReleaseDate)",
                            new { entity.Id, entity.Title, entity.Owner, entity.Genre , ReleaseDate = DateTime.Now});

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateMovie(Movie entity)
        {
            using var connection = new SqlConnection
                         (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                    ("UPDATE Movie SET Title=@Title, Owner = @Owner, Genre = @Genre, ReleaseDate=@ReleaseDate WHERE Id = @Id",
                            new { entity.Id, entity.Title, entity.Owner, entity.Genre, ReleaseDate = DateTime.Now });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteMovie(string title)
        {
            using var connection = new SqlConnection
                         (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM Movie WHERE Title = @Title",
                new { Title = title });

            if (affected == 0)
                return false;

            return true;
        }
    }
}