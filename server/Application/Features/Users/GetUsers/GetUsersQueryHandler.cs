using Application.Exceptions;
using Application.Features.Users.Common;
using Application.Utils;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Features.Users.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersResponse>
    {
        private readonly DataContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public GetUsersQueryHandler(DataContext context, IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {

            if(_context.Users == null)
            {
                 throw new NotFoundException("Users not found");
            }

            int authUserId = _authenticatedUserService.UserId;

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-request.Params.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-request.Params.MinAge));

            var users = _context.Users
               .AsQueryable()
               .Where(u => u.Id != authUserId)
               .Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob)
               .Where(u => string.IsNullOrEmpty(request.Params.Gender) || u.Gender == request.Params.Gender)
               .Where(u => string.IsNullOrEmpty(request.Params.Country) || u.Country == request.Params.Country)
               .OrderByDescending(u => request.Params.OrderBy == "created" ? u.Created : u.LastActive)
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
                .CreateAsync(users, request.Params.PageNumber, request.Params.PageSize);

            return new GetUsersResponse
            {
                Users = pagedUsers
            };
        }
    }
}
