using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Admin.AdminUserDelete
{
    public class AdminUserDeleteCommandHandler : IRequestHandler<AdminUserDeleteCommand, AdminUserDeleteResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserManager _userManager;

        public AdminUserDeleteCommandHandler(IApplicationDbContext context, IUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<AdminUserDeleteResponse> Handle(AdminUserDeleteCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(new object[] { request.AppUserId }, cancellationToken)
                ?? throw new NotFoundException("User not found.");

            var authUser = await _context.Users.FindAsync(new object[] { request.AuthUserId }, cancellationToken)
                ?? throw new NotFoundException("Auth user not found.");

            // Only an Admin is authorized for deleting a user. This implementation includes multiple role checks (UserManager, DbContext, and API Policy)
            // to ensure robust security. While this might seem excessive, it's designed primarily for educational purposes to demonstrate various validation techniques.
            // The endpoint is not expected to be heavily used, but the approach highlights a thorough understanding of security practices and redundancy for critical operations.
            bool isAdmin = await _userManager.IsInRoleAsync(authUser, "Admin")
                && await _context.Users.AnyAsync(u => u.Id == request.AuthUserId && u.UserRoles.Any(r => r.Role.Name == "Admin"), cancellationToken)
                && request.AuthUserRoles.Contains("Admin");

            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("Unauthorized, only an Admin can delete a user.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            return new AdminUserDeleteResponse { UserDeleted = true };
        }
    }
}
