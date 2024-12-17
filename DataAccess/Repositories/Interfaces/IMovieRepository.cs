using Common.Models;

namespace DataAccess.Repositories.Interfaces;

public interface IMovieRepository
{
    Task AddMovieAsync(Movie? movie);
    Task<Movie?> GetMovieByIdAsync(int id);

}