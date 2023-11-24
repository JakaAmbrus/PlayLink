using Application.Features.Users.Common;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.GetUsersForSearchBar
{
    public class GetUsersForSearchBarQueryHandler : IRequestHandler<GetUsersForSearchBarQuery, GetUsersForSearchBarResponse>
    {
        private readonly DataContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public GetUsersForSearchBarQueryHandler(DataContext context, IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }
        public async Task<GetUsersForSearchBarResponse> Handle(GetUsersForSearchBarQuery request, CancellationToken cancellationToken)
        {

            int authUserId = _authenticatedUserService.UserId;

            var users = await _context.Users
               .Where(u => u.Id != authUserId)
               .OrderByDescending(u => u.LastActive)
               .Select(u => new SearchUserDto
               {
                   Username = u.UserName,
                   FullName = u.FullName,
                   ProfilePictureUrl = u.ProfilePictureUrl
               }).ToListAsync(cancellationToken);

            return new GetUsersForSearchBarResponse
            {
                Users = users
            };
        }
    }
}
