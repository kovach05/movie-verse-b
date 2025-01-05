namespace Movie_Verse.Controllers;

using Microsoft.AspNetCore.Mvc;
using Movie_Verse.Services;

[ApiController]
[Route("api/tv")]
public class TvShowsController : ControllerBase
{
    private readonly TVService _tvService;

    // Конструктор для інжектування TVService
    public TvShowsController(TVService tvService)
    {
        _tvService = tvService;
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularTvShows()
    {
        try
        {
            var tvShows = await _tvService.GetPopularTVShowsAsync();
            return Ok(tvShows);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

}