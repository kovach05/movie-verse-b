using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Authorize]
[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetProfile()
    {
        Console.WriteLine("GetProfile endpoint was hit!");

        var userId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            Console.WriteLine("User ID not found in token.");
            return Unauthorized(new { message = "User ID not found in token." });
        }

        var profile = await _userService.GetUserProfileAsync(userId);
        if (profile == null)
        {
            Console.WriteLine("User not found.");
            return NotFound(new { message = "User not found." });
        }

        Console.WriteLine(" User found, returning profile.");
        return Ok(profile);
    }

    
    [HttpGet("all")]
    [AllowAnonymous] // Дозволяє доступ без авторизації (можна прибрати після тестування)
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }
}