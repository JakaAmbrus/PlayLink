using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services
{
    internal class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                // because we are using JWT, we can get the user ID from the token but the accessor
                // gives us back a string, so we need to parse it to an int
                var userIdAsString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdAsString, out int userId))
                {
                    return userId;
                }
                else
                {
                    throw new InvalidOperationException("User ID is not correct.");
                }
            }
        }

        public IEnumerable<string> UserRoles
        {
            get
            {
                //this gets us the role stored in the JWT
                var roleClaims = _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role);
                return roleClaims?.Select(claim => claim.Value) ?? Enumerable.Empty<string>();
            }
        }
    }
}


