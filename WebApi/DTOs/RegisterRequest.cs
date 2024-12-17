using System.ComponentModel.DataAnnotations;

namespace Movie_Verse.DTOs;

public class RegisterRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string? Username { get; init; }
    public string Password { get; init; }
    //public string RoleName { get; init; }
}