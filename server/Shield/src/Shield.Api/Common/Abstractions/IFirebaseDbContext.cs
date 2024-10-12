using Google.Cloud.Firestore;

namespace Shield.Api.Common.Abstractions;

public interface IFirebaseDbContext
{
    Task<DocumentSnapshot> GetUserByIdAsync(string userId);

    Task AddUserAsync(string userId, string username, List<string> initialRoles);
    
    Task AddRolesAsync(string userId, List<string> rolesToAdd);
    
    Task RemoveRoleAsync(string userId, string roleToRemove);
}