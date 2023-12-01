using Application.Exceptions;
using Application.Features.Users.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.GetUserByUsername
{
    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, GetUserByUsernameResponse>
    {
        private readonly IApplicationDbContext _context;
        public GetUserByUsernameQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetUserByUsernameResponse> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException($"The user by the username: {request.Username} not found ");
            }

            var profileUserDto = new ProfileUserDto
            {
                AppUserId = user.Id,
                Username = user.UserName,
                Gender = user.Gender,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Country = user.Country,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Description = user.Description
            };

            return new GetUserByUsernameResponse { User = profileUserDto };
        }
    }
}
