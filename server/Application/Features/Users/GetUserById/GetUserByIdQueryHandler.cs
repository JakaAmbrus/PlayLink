using Application.Exceptions;
using Application.Features.Users.Common;
using Application.Interfaces;
using Application.Utils;
using MediatR;

namespace Application.Features.Users.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetUserByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken)
                ?? throw new NotFoundException($"User with the id {request.Id} not found.");

            var userDto = new UsersDto
            {
                AppUserId = user.Id,
                Username = user.UserName,
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
