using Application.Exceptions;
using Application.Features.Admin.Common;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Application.Utils;
using MediatR;
using System.Data;

namespace Application.Features.Admin.AdminGetUsers
{
    public class AdminGetUsersQueryHandler : IRequestHandler<AdminGetUsersQuery, AdminGetUsersResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserManager _userManager;

        public AdminGetUsersQueryHandler(IApplicationDbContext context, IUserManager userManager) 
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<AdminGetUsersResponse> Handle(AdminGetUsersQuery request, CancellationToken cancellationToken)
        {
            var authUser = await _context.Users.FindAsync(new object[] { request.AuthUserId}, cancellationToken) 
                ?? throw new NotFoundException("Auth user not found.");

            bool isAdmin = await _userManager.IsInRoleAsync(authUser, "Admin");
            
            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("Unauthorized, only an Admin can make this request.");
            }

            var usersQuery = _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.Id != request.AuthUserId)
                .OrderByDescending(u => u.Created)
                .Select(u => new UserWithRolesDto
                {
                    AppUserId = u.Id,
                    Username = u.UserName,
                    Gender = u.Gender,
                    FullName = u.FullName,
                    IsModerator = u.UserRoles.Any(ur => ur.Role.Name == "Moderator"),
                    ProfilePictureUrl = u.ProfilePictureUrl,
                    Created = u.Created
                });

            var pagedUsers = await PagedList<UserWithRolesDto>
                .CreateAsync(usersQuery, request.Params.PageNumber, request.Params.PageSize);

            return new AdminGetUsersResponse { Users = pagedUsers };
        }
    }
}
