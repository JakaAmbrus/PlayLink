using Application.Exceptions;
using Application.Interfaces;

namespace Application.Services
{
    public class AuthenticatedUserUsernameService : IAuthenticatedUserUsernameService
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IApplicationDbContext _context;

        public AuthenticatedUserUsernameService(IAuthenticatedUserService authenticatedUserService, IApplicationDbContext context)
        {
            _authenticatedUserService = authenticatedUserService;
            _context = context;
        }

        public async Task<string> GetUsernameByIdAsync()
        {
            int authUserId = _authenticatedUserService.UserId;

            var user = await _context.Users.FindAsync(authUserId)
                ?? throw new NotFoundException("User not found");

            return user.UserName;
        }
    }
}
