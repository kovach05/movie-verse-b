using Microsoft.AspNetCore.Mvc;

namespace Movie_Verse.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Backend is connected!");
    }
}