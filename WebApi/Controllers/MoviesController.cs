using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

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
}