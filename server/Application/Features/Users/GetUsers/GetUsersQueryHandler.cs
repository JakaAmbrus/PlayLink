using Application.Features.Users.Common;
using Application.Interfaces;
using Application.Utils;
using MediatR;

namespace Application.Features.Users.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetUsersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var minDob = DateTime.Today.AddYears(-request.Params.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-request.Params.MinAge);

            var users = _context.Users
               .AsQueryable()
               .Where(u => u.Id != request.AuthUserId)
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
                   Age = DateOnly.FromDateTime(u.DateOfBirth).CalculateAge(),
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
