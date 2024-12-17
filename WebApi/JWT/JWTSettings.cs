namespace Movie_Verse.JWT;

public class JWTSettings
{
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? Key { get; set; }
    public int ExpiryMinutes { get; set; }
}