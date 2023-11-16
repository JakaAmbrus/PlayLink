using Application.Exceptions;
using Application.Features.Users.Common;
using Application.Utils;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersResponse>
    {
        private readonly DataContext _context;

        public GetUsersQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {

        if(_context.Users == null)
            {
                throw new NotFoundException("Users not found");
            }

            var users = await _context.Users
               .Select(u => new UsersDto
               {
                   AppUserId = u.Id,
                   Username = u.UserName,
                   FullName = u.FullName,
                   Age = u.DateOfBirth.CalculateAge(),
                   Country = u.Country,
                   City = u.City,
                   ProfilePictureUrl = u.ProfilePictureUrl
               }).ToListAsync(cancellationToken);

        return new GetUsersResponse { Users = users };
        }
    }
}
