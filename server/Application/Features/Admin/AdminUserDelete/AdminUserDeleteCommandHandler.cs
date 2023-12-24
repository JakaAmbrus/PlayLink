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
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public AdminUserDeleteCommandHandler(IApplicationDbContext context, IUserManager userManager, ICacheInvalidationService cacheInvalidationService)
        {
            _context = context;
            _userManager = userManager;
            _cacheInvalidationService = cacheInvalidationService;
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

            using (var transaction = await _context.BeginTransactionAsync(cancellationToken))
            {
                try
                {

                    var userConnections = _context.Connections.Where(c => c.Username == user.UserName).ToList();
                    _context.Connections.RemoveRange(userConnections);

                    var posts = _context.Posts.Where(p => p.AppUserId == request.AppUserId).ToList();
                    _context.Posts.RemoveRange(posts);

                    var sentRequests = _context.FriendRequests.Where(fr => fr.SenderId == request.AppUserId).ToList();
                    var receivedRequests = _context.FriendRequests.Where(fr => fr.ReceiverId == request.AppUserId).ToList();
                    _context.FriendRequests.RemoveRange(sentRequests.Concat(receivedRequests));

                    var friendshipsAsUser1 = _context.Friendships.Where(f => f.User1Id == request.AppUserId).ToList();
                    var friendshipsAsUser2 = _context.Friendships.Where(f => f.User2Id == request.AppUserId).ToList();
                    _context.Friendships.RemoveRange(friendshipsAsUser1.Concat(friendshipsAsUser2));

                    var sentMessages = _context.PrivateMessages.Where(msg => msg.SenderId == request.AppUserId).ToList();
                    _context.PrivateMessages.RemoveRange(sentMessages);

                    var receivedMessages = _context.PrivateMessages.Where(msg => msg.RecipientId == request.AppUserId).ToList();
                    _context.PrivateMessages.RemoveRange(receivedMessages);

                    _context.Users.Remove(user);

                    await _context.SaveChangesAsync(cancellationToken);

                    _cacheInvalidationService.InvalidateSearchUserCache();
                    _cacheInvalidationService.InvalidateNearestBirthdayUsersCache();

                    await transaction.CommitAsync(cancellationToken);

                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }

            return new AdminUserDeleteResponse { UserDeleted = true };
        }
    }
}
