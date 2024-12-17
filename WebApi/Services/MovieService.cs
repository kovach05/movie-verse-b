// using Common.Models;
// using DataAccess.Repositories.Interfaces;
// using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
// using Movie_Verse.DTOs;
// using Movie_Verse.Interfaces;
//
// namespace Movie_Verse.Services;
//
// public class MovieService : IMovieService
// {
//     private readonly IMovieRepository _movieRepository;
//
//     public MovieService(IMovieRepository movieRepository)
//     {
//         _movieRepository = movieRepository;
//     }
//
//     public async Task UploadMovieAsync(IFormFile video, string title, string genre, int year)
//     {
//         using var memoryStream = new MemoryStream();
//         await video.CopyToAsync(memoryStream);
//
//         var movie = new Movie
//         {
//             Title = title,
//             Genre = genre,
//             Year = year,
//             Video = memoryStream.ToArray()
//         };
//
//         await _movieRepository.AddMovieAsync(movie);
//     }
//
//     public async Task<byte[]?> GetMovieVideoAsync(int id)
//     {
//         var movie = await _movieRepository.GetMovieByIdAsync(id);
//         return movie?.Video;
//     }
// }