using Application.Exceptions;
using Application.Features.Authentication.Common;
using Application.Features.Users.Common;
using Application.Utils;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.GetUserByUsername
{
    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, GetUserByUsernameResponse>
    {
        private readonly DataContext _context;
        public GetUserByUsernameQueryHandler(DataContext context)
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

            var userDto = new UsersDto
            {
                AppUserId = user.Id,
                Username = user.UserName,
                Gender = user.Gender,
                FullName = user.FullName,
                Age = user.DateOfBirth.CalculateAge(),
                Country = user.Country,
                City = user.City,
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            return new GetUserByUsernameResponse { User = userDto};
        }
    }
}
