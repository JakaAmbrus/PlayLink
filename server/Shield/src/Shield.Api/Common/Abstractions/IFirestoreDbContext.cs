using Shield.Api.Common.Models;

namespace Shield.Api.Common.Abstractions;

public interface IFirestoreDbContext
{
    Task<User> GetUserByIdAsync(string userId);

    Task AddUserAsync(string userId, string username, long socialId);
    
    Task AddRolesAsync(string userId, List<string> rolesToAdd);
    
    Task RemoveRoleAsync(string userId, string roleToRemove);

    Task DeleteUserAsync(string userId);
}