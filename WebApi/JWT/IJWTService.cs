using Common.Models;

namespace Movie_Verse.JWT;

public interface IJWTService
{
    string GenerateToken(User user);
}