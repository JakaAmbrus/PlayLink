using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Features.Authentication.Common;
using Social.Application.Interfaces;

namespace Social.Application.Features.Authentication.GuestUserLogin
{
    public class GuestUserLoginCommandHandler : IRequestHandler<GuestUserLoginCommand, GuestUserLoginResponse>
    {
        private readonly ISocialDbContext _context;

        public GuestUserLoginCommandHandler(ISocialDbContext context)
        {
            _context = context;
        }

        public async Task<GuestUserLoginResponse> Handle(GuestUserLoginCommand request, CancellationToken cancellationToken)
        {
            var memberUsernames = new List<string> { "testone", "testtwo", "testthree" };
            var moderatorUsernames = new List<string> { "modone", "modtwo", "modthree" };

            var usernames = request.Role.Equals("Moderator", StringComparison.OrdinalIgnoreCase) ? moderatorUsernames : memberUsernames;

            var user = await _context.Users
                // .Where(x => usernames.Contains(x.Username))
                .OrderBy(x => x.LastActive) //I want to avoid multiple users logging in with the same account
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Guest user not found");

            return new GuestUserLoginResponse
            {
                User = new UserDto
                {
                    // Username = user.Username,
                    // Token = await _tokenService.CreateToken(user),
                    FullName = user.FullName,
                    Gender = user.Gender,
                    ProfilePictureUrl = user.ProfilePictureUrl
                }
            };
        }
    }
}
