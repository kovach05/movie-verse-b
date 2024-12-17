
namespace Movie_Verse.Interfaces;

public interface IMovieService
{
    Task UploadMovieAsync(IFormFile video, string title, string genre, int year);
    Task<byte[]?> GetMovieVideoAsync(int id);
}