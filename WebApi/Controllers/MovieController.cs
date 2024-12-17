// using Microsoft.AspNetCore.Mvc;
// using Microsoft.OpenApi.Any;
// using Movie_Verse.Interfaces;
//
// namespace Movie_Verse.Controllers;
//
// [ApiController]
// [Route("api/[controller]")]
// public class MovieController : ControllerBase
// {
//     private readonly IMovieService _movieService;
//
//     public MovieController(IMovieService movieService)
//     {
//         _movieService = movieService;
//     }
//     
//     [HttpPost("upload-video")]
//     public async Task<IActionResult> UploadVideo([FromForm] IFormFile video, [FromForm] string title, [FromForm] string genre, [FromForm] int year)
//     {
//         if (video == null || video.Length == 0) return BadRequest("Video file is missing.");
//         await _movieService.UploadMovieAsync(video, title, genre, year);
//         return Ok("Video uploaded successfully.");
//     }
//
//     [HttpGet("get-video/{id}")]
//     public async Task<IActionResult> GetVideo(int id)
//     {
//         var video = await _movieService.GetMovieVideoAsync(id);
//         if (video == null) return NotFound("Video not found.");
//         return File(video, "video/mp4");
//     }
// }