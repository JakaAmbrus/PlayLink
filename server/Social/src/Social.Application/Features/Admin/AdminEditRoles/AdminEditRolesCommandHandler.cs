using Social.Domain.Exceptions;
using MediatR;
using Social.Application.Interfaces;
using Social.Domain.Enums;

namespace Social.Application.Features.Admin.AdminEditRoles
{
    public class AdminEditRolesCommandHandler : IRequestHandler<AdminEditRolesCommand, AdminEditRolesResponse>
    {
        private readonly ISocialDbContext _context;

        public AdminEditRolesCommandHandler(ISocialDbContext context)
        {
            _context = context;
        }

        public async Task<AdminEditRolesResponse> Handle(AdminEditRolesCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(new object[] { request.AppUserId }, cancellationToken)
                ?? throw new NotFoundException("User to edit not found");

            var authUser = await _context.Users.FindAsync(new object[] { request.AuthUserId }, cancellationToken)
                ?? throw new NotFoundException("Authenticated Admin user not found");
            //
            // bool isAdmin = await _userManager.IsInRoleAsync(authUser, "Admin");
            //
            // if (!isAdmin)
            // {
            //     throw new UnauthorizedException("Unauthorized, only an Admin can make this request");
            // }
            //
            // bool isModerator = await _userManager.IsInRoleAsync(user, Role.Moderator.ToString());
            //
            // if (!isModerator)
            // {
            //     await _userManager.AddToRoleAsync(user, Role.Moderator.ToString());
            // }
            //
            // else
            // {
            //     await _userManager.RemoveFromRoleAsync(user, Role.Moderator.ToString());
            // }
            // Todo

            await _context.SaveChangesAsync(cancellationToken);

            return new AdminEditRolesResponse
            {
                RoleEdited = true
            };
        }
    }
}
