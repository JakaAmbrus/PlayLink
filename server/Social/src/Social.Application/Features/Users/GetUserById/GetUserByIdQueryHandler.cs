using Social.Application.Utils;
using Social.Domain.Exceptions;
using MediatR;
using Social.Application.Features.Users.Common;
using Social.Application.Interfaces;

namespace Social.Application.Features.Users.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
    {
        private readonly ISocialDbContext _context;

        public GetUserByIdQueryHandler(ISocialDbContext context)
        {
            _context = context;
        }

        public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.Id)
                ?? throw new NotFoundException($"User not found.");

            var userDto = new UsersDto
            {
                AppUserId = user.Id,
                Username = user.Username,
                Gender = user.Gender,
                FullName = user.FullName,
                Age = user.DateOfBirth.CalculateAge(),
                Country = user.Country,
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            return new GetUserByIdResponse { User = userDto };
        }
    }
}
