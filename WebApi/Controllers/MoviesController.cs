using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Common.Models;

[ApiController]
[Route("api/movies")]
public class MoviesController : ControllerBase
{
    private readonly TMDBService _tmdbService;
    private readonly ILogger<MoviesController> _logger;

    public MoviesController(TMDBService tmdbService, ILogger<MoviesController> logger)
    {
        _tmdbService = tmdbService;
        _logger = logger;
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularMovies()
    {
        try
        {
            var movies = await _tmdbService.GetPopularMoviesAsync();
            
            // Логування даних про фільми
            _logger.LogInformation("Received movies data: {Movies}", JsonSerializer.Serialize(movies));

            return Ok(movies);
        }
        catch (Exception ex)
        {
            // Логування помилки
            _logger.LogError("Error occurred: {Error}", ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieById(int id)
    {
        try
        {
            var movie = await _tmdbService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound($"Movie with ID {id} not found.");
            }
            return Ok(movie);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"External API error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpGet("{id}/trailer")]
    public async Task<IActionResult> GetMovieTrailer(int id)
    {
        try
        {
            var trailerKey = await _tmdbService.GetMovieTrailerKeyAsync(id);
            if (string.IsNullOrEmpty(trailerKey))
            {
                return NotFound("Trailer not found.");
            }
    
            return Ok(new { TrailerKey = trailerKey });
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(404, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}/credits")]
    public async Task<IActionResult> GetMovieCredits(int id)
    {
        try
        {
            var credits = await _tmdbService.GetMovieCreditsAsync(id); // Використовуємо метод з TMDBService
            if (credits == null || !credits.Cast.Any())
            {
                return NotFound("No cast information found for this movie.");
            }

            return Ok(credits.Cast.Take(10)); // Повертаємо перших 10 акторів
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"External API error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // [Route("api/movies/popular")]
    // [HttpGet]
    // public async Task<IActionResult> GetPopularMovies()
    // {
    //     try
    //     {
    //         // Отримання даних з бази або TMDB API
    //         var movies = await _movieService.GetPopularMoviesAsync();
    //         return Ok(movies);
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, new { error = ex.Message });
    //     }
    // }

    
}