using Application.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Admin.AdminEditRoles
{
    public class AdminEditRolesCommandHandler : IRequestHandler<AdminEditRolesCommand, AdminEditRolesResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserManager _userManager;

        public AdminEditRolesCommandHandler(IApplicationDbContext context, IUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<AdminEditRolesResponse> Handle(AdminEditRolesCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(new object[] { request.AppUserId }, cancellationToken)
                ?? throw new NotFoundException("User to edit not found.");

            var authUser = await _context.Users.FindAsync(new object[] { request.AuthUserId }, cancellationToken)
                ?? throw new NotFoundException("Auth user not found.");

            bool isAdmin = await _userManager.IsInRoleAsync(authUser, "Admin");

            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("Unauthorized, only an Admin can make this request.");
            }

            bool isModerator = await _userManager.IsInRoleAsync(user, "Moderator");

            if (request.AssignModeratorRole && !isModerator)
            {
                await _userManager.AddToRoleAsync(user, "Moderator");
            }
          
            else if (!request.AssignModeratorRole && isModerator)
            {
                await _userManager.RemoveFromRoleAsync(user, "Moderator");
            }

            else 
            {
                return new AdminEditRolesResponse
                {
                    RoleEdited = false
                };
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new AdminEditRolesResponse
            {
                RoleEdited = true
            };
        }
    }
}
