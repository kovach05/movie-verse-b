using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Movie_Verse.JWT;

public class JWTService : IJWTService
{
    private readonly JWTSettings _jwtSettings;
    private readonly ILogger<JWTService> _logger;

    public JWTService(IOptions<JWTSettings> jwtSettings, ILogger<JWTService> logger)
    {
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public string GenerateToken(User user)
    {
        _logger.LogInformation("Розпочато генерацію JWT токена для користувача: {Username}", user.UserName);

        if (string.IsNullOrEmpty(_jwtSettings.Key))
        {
            _logger.LogError("JWT Key порожній.");
            throw new InvalidOperationException("JWT Key cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(_jwtSettings.Issuer))
        {
            _logger.LogError("JWT Issuer порожній.");
            throw new InvalidOperationException("JWT Issuer cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(_jwtSettings.Audience))
        {
            _logger.LogError("JWT Audience порожній.");
            throw new InvalidOperationException("JWT Audience cannot be null or empty.");
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        _logger.LogInformation("Створення токена з параметрами: Issuer={Issuer}, Audience={Audience}, Expiry={ExpiryMinutes}",
            _jwtSettings.Issuer, _jwtSettings.Audience, 30);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        _logger.LogInformation("JWT токен успішно створено для користувача: {Username}", user.UserName);
        return jwt;
    }
}