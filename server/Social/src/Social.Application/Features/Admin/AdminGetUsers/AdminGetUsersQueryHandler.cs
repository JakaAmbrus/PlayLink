using MediatR;
using Social.Application.Features.Admin.Common;
using Social.Application.Interfaces;
using Social.Application.Utils;
using Social.Domain.Exceptions;

namespace Social.Application.Features.Admin.AdminGetUsers
{
    public class AdminGetUsersQueryHandler : IRequestHandler<AdminGetUsersQuery, AdminGetUsersResponse>
    {
        private readonly ISocialDbContext _context;

        public AdminGetUsersQueryHandler(ISocialDbContext context) 
        {
            _context = context;
        }

        public async Task<AdminGetUsersResponse> Handle(AdminGetUsersQuery request, CancellationToken cancellationToken)
        {
            var authUser = await _context.Users.FindAsync(new object[] { request.AuthUserId}, cancellationToken) 
                ?? throw new NotFoundException("Authorized user not found");

            // bool isAdmin = await _userManager.IsInRoleAsync(authUser, "Admin");
            //
            // if (!isAdmin)
            // {
            //     throw new UnauthorizedException("Unauthorized, only an Admin can make this request");
            // }
            //
            // var usersQuery = _context.Users
            //     .Include(u => u.UserRoles)
            //         .ThenInclude(ur => ur.Roles)
            //     .Where(u => u.Id != request.AuthUserId)
            //     .OrderByDescending(u => u.Created)
            //     .Select(u => new UserWithRolesDto
            //     {
            //         AppUserId = u.Id,
            //         Username = u.Username,
            //         Gender = u.Gender,
            //         FullName = u.FullName,
            //         IsModerator = u.UserRoles.Any(ur => ur.Roles.Name == Roles.Moderator.ToString()),
            //         ProfilePictureUrl = u.ProfilePictureUrl,
            //         Created = u.Created
            //     });
            //
            // var pagedUsers = await PagedList<UserWithRolesDto>
            //     .CreateAsync(usersQuery, request.Params.PageNumber, request.Params.PageSize);

            return new AdminGetUsersResponse {  };
        }
    }
}
