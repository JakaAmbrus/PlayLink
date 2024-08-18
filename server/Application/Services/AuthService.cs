using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services
{
    internal class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationDbContext _context;

        public AuthService(IHttpContextAccessor httpContextAccessor, IApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public int GetCurrentUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdString, out var userId))
            {
                throw new UnauthorizedException("User not authenticated");
            }

            return userId;
        }

        public IEnumerable<string> GetCurrentUserRoles()
        {
            var roleClaims = _httpContextAccessor.HttpContext?.User?
                .FindAll(ClaimTypes.Role)
                .Select(claim => claim.Value);
            
            return roleClaims ?? Enumerable.Empty<string>();
        }
        
        public async Task<string> GetUsernameByIdAsync(CancellationToken cancellationToken)
        {
            var authUserId = GetCurrentUserId();

            var user = await _context.Users.FindAsync(new object[] { authUserId }, cancellationToken)
                       ?? throw new NotFoundException("Authenticated user not found from JWT.");

            return user.UserName;
        }
    }
}


