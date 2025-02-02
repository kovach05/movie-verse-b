public interface IUserService
{
    Task<UserProfileDto> GetUserProfileAsync(string userId);
    Task<List<UserProfileDto>> GetAllUsersAsync();
}