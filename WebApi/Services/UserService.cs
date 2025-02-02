using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileDto> GetUserProfileAsync(string userId)
    {
        Console.WriteLine($"Querying database for User ID: {userId}");

        var profile = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserProfileDto
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email
            })
            .FirstOrDefaultAsync();

        if (profile == null)
        {
            Console.WriteLine($"No user found for User ID: {userId}");
        }

        return profile;
    }
    
    public async Task<List<UserProfileDto>> GetAllUsersAsync()
    {
        return await _context.Users
            .Select(u => new UserProfileDto
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email
            })
            .ToListAsync();
    }
}