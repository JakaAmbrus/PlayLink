using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Shared.Core.Security;
using Social.Application.Features.Users.Common;
using Social.Application.Interfaces;
using Social.Domain.Exceptions;

namespace Social.Application.Features.Users.GetUserByUsername
{
    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, GetUserByUsernameResponse>
    {
        private readonly ISocialDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public GetUserByUsernameQueryHandler(ISocialDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<GetUserByUsernameResponse> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken)
                ?? throw new NotFoundException($"The user by the username: {request.Username} not found ");

            bool isModerator = request.AuthUserRoles.Contains(Roles.Moderator);
            bool isCurrentUser = request.AuthUserId == user.Id;

            var profileUserDto = new ProfileUserDto
            {
                AppUserId = user.Id,
                Username = user.Username,
                Gender = user.Gender,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Country = user.Country,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Description = user.Description,
                Created = user.Created,
                LastActive = user.LastActive,
                Authorized = isModerator && !isCurrentUser // Moderation view is only seen on other profiles
            };

            return new GetUserByUsernameResponse { User = profileUserDto };
        }
    }
}
