using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Shared.Core.Security;

public interface IAuthContextService
{
    string? GetUserId();
    string? GetUsername();
    int GetSocialId();
    IEnumerable<string> GetUserRoles();
}

public class AuthContextService : IAuthContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserId()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    
    public int GetSocialId()
    {
        var userIdString =  _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.PrimarySid)?.Value;
        
        if (!int.TryParse(userIdString, out var userId))
        {
            throw new InvalidOperationException("Social ID claim is missing.");
        }

        return userId;
    }
    
    public string? GetUsername()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
    }

    public IEnumerable<string> GetUserRoles()
    {
        var roleClaims = _httpContextAccessor.HttpContext?.User?
            .FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value);
            
        return roleClaims ?? [];
    }
}