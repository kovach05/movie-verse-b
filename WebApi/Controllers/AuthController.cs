using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movie_Verse.DTOs;
using Movie_Verse.Interfaces;

namespace Movie_Verse.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        _logger.LogInformation("Розпочато реєстрацію користувача з ім'ям: {Username}", request.Username);

        var result = await _authService.RegisterAsync(request);
        if (result != "User registered successfully")
        {
            _logger.LogWarning("Реєстрація користувача {Username} не вдалася: {Result}", request.Username, result);
            return BadRequest(result);
        }

        _logger.LogInformation("Користувач {Username} успішно зареєстрований.", request.Username);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Спроба входу користувача: {Username}", request.Username);

        var result = await _authService.LoginAsync(request);

        if (result == "Invalid credentials")
        {
            _logger.LogWarning("Невдала спроба входу користувача: {Username}. Невірні облікові дані.", request.Username);
            return Unauthorized(result);
        }

        _logger.LogInformation("Користувач {Username} успішно увійшов у систему.", request.Username);
        return Ok(new { Token = result });
    }
}