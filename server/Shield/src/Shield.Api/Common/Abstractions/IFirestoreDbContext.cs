using Shield.Api.Common.Models;

namespace Shield.Api.Common.Abstractions;

public interface IFirestoreDbContext
{
    Task<User> GetUserByIdAsync(string userId);

    Task AddUserAsync(string userId, string username, long socialId);

    Task UpdateUserRolesAsync(string userId, List<string> roles);

    Task DeleteUserAsync(string userId);
}