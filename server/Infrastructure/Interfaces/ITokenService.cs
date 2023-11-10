using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
