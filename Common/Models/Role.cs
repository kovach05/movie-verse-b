using Microsoft.AspNetCore.Identity;

namespace Common.Models;

public class Role : IdentityRole<string>
{
    public ICollection<UserRole>? UserRoles { get; set; }
}