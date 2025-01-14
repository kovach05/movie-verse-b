using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Common.Models;
using Movie_Verse.Services;

[ApiController]
[Route("api/tv")]
public class TvShowsController : ControllerBase
{
    private readonly TVService _tvService;
    private readonly ILogger<TvShowsController> _logger;

    public TvShowsController(TVService tvService, ILogger<TvShowsController> logger)
    {
        _tvService = tvService;
        _logger = logger;
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularTvShows()
    {
        try
        {
            var tvShows = await _tvService.GetPopularTVShowsAsync();

            // Логування даних про серіали
            _logger.LogInformation("Received TV shows data: {TVShows}", JsonSerializer.Serialize(tvShows));

            return Ok(tvShows);
        }
        catch (Exception ex)
        {
            // Логування помилки
            _logger.LogError("Error occurred: {Error}", ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTvShowById(int id)
    {
        try
        {
            var tvShow = await _tvService.GetTvShowByIdAsync(id);
            if (tvShow == null)
            {
                return NotFound($"TV show with ID {id} not found.");
            }

            return Ok(tvShow);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("External API error: {Error}", ex.Message);
            return StatusCode(500, $"External API error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {Error}", ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    [HttpGet("{id}/trailer")]
    public async Task<IActionResult> GetTvShowTrailer(int id)
    {
        try
        {
            var trailerKey = await _tvService.GetTvShowTrailerAsync(id);

            if (string.IsNullOrEmpty(trailerKey))
            {
                return NotFound($"Trailer for TV show with ID {id} not found.");
            }

            return Ok(new { TrailerKey = trailerKey });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("External API error: {Error}", ex.Message);
            return StatusCode(500, $"External API error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {Error}", ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}/credits")]
    public async Task<IActionResult> GetTvShowCredits(int id)
    {
        try
        {
            var credits = await _tvService.GetTvShowCreditsAsync(id);

            if (credits == null || credits.Count == 0)
            {
                return NotFound($"Credits for TV show with ID {id} not found.");
            }

            return Ok(credits);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("External API error: {Error}", ex.Message);
            return StatusCode(500, $"External API error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {Error}", ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}