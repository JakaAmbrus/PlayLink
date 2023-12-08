using Application.Features.Users.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.GetUsersForSearchBar
{
    public class GetUsersForSearchBarQueryHandler : IRequestHandler<GetUsersForSearchBarQuery, GetUsersForSearchBarResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetUsersForSearchBarQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<GetUsersForSearchBarResponse> Handle(GetUsersForSearchBarQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users
               .Where(u => u.Id != request.AuthUserId)
               .OrderByDescending(u => u.LastActive)
               .Select(u => new SearchUserDto
               {
                   AppUserId = u.Id,
                   Username = u.UserName,
                   FullName = u.FullName,
                   ProfilePictureUrl = u.ProfilePictureUrl,
                   Gender = u.Gender,
               }).ToListAsync(cancellationToken);

            return new GetUsersForSearchBarResponse
            {
                Users = users
            };
        }
    }
}
