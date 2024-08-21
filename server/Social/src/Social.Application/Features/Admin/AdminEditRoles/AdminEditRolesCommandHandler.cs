using Social.Domain.Exceptions;
using MediatR;
using Social.Application.Interfaces;

namespace Social.Application.Features.Admin.AdminEditRoles
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
                ?? throw new NotFoundException("User to edit not found");

            var authUser = await _context.Users.FindAsync(new object[] { request.AuthUserId }, cancellationToken)
                ?? throw new NotFoundException("Authenticated Admin user not found");

            bool isAdmin = await _userManager.IsInRoleAsync(authUser, "Admin");

            if (!isAdmin)
            {
                throw new UnauthorizedException("Unauthorized, only an Admin can make this request");
            }

            bool isModerator = await _userManager.IsInRoleAsync(user, "Moderator");

            if (!isModerator)
            {
                await _userManager.AddToRoleAsync(user, "Moderator");
            }
            
            else
            {
                await _userManager.RemoveFromRoleAsync(user, "Moderator");
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new AdminEditRolesResponse
            {
                RoleEdited = true
            };
        }
    }
}
