using Application.Exceptions;
using Application.Features.Authentication.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Authentication.GuestUserLogin
{
    public class GuestUserLoginCommandHandler : IRequestHandler<GuestUserLoginCommand, GuestUserLoginResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public GuestUserLoginCommandHandler(IApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<GuestUserLoginResponse> Handle(GuestUserLoginCommand request, CancellationToken cancellationToken)
        {
            var memberUsernames = new List<string> { "testone", "testtwo", "testthree" };
            var moderatorUsernames = new List<string> { "testmodone", "testmodtwo", "testmodtre" };

            var usernames = request.Role.Equals("Moderator", StringComparison.OrdinalIgnoreCase) ? moderatorUsernames : memberUsernames;

            var user = await _context.Users
                .Where(x => usernames.Contains(x.UserName))
                .OrderBy(x => x.LastActive) //I want to avoid multiple users logging in with the same account
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Guest user not found");

            return new GuestUserLoginResponse
            {
                User = new UserDto
                {
                    Username = user.UserName,
                    Token = await _tokenService.CreateToken(user),
                    FullName = user.FullName,
                    Gender = user.Gender,
                    ProfilePictureUrl = user.ProfilePictureUrl
                }
            };
        }
    }
}
