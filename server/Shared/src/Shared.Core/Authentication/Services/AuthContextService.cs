using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shared.Core.Authentication.Interfaces;

namespace Shared.Core.Authentication.Services;

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