using Social.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Social.Application.Interfaces
{
    public interface IUserManager
    {
        Task<IdentityResult> CreateAsync(AppUser user, string password);
        Task<AppUser> FindByIdAsync(string userId);
        Task<AppUser> FindByUsernameAsync(string username);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<bool> IsInRoleAsync(AppUser user, string role);
        Task<IdentityResult> AddToRoleAsync(AppUser user, string role);
        Task<IdentityResult> RemoveFromRoleAsync(AppUser user, string role);
    }
}
