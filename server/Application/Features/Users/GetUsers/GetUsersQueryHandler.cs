using Application.Exceptions;
using Application.Features.Users.Common;
using Application.Utils;
using Infrastructure.Data;
using MediatR;

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

            var users = _context.Users
               .AsQueryable()
               .OrderBy(u => u.FullName)
               .Select(u => new UsersDto
               {
                   AppUserId = u.Id,
                   Username = u.UserName,
                   Gender = u.Gender,
                   FullName = u.FullName,
                   Age = u.DateOfBirth.CalculateAge(),
                   Country = u.Country,
                   ProfilePictureUrl = u.ProfilePictureUrl
               });

            var pagedUsers = await PagedList<UsersDto>
                .CreateAsync(users, request.PaginationParams.PageNumber, request.PaginationParams.PageSize);

            return new GetUsersResponse
            {
                Users = pagedUsers
            };
        }
    }
}
