using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Interfaces;

namespace Social.Application.Features.Users.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public DeleteUserCommandHandler(IApplicationDbContext context, ICacheInvalidationService cacheInvalidationService)
        {
            _context = context;
            _cacheInvalidationService = cacheInvalidationService;
        }

        public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.AuthUserId, cancellationToken)
                ?? throw new NotFoundException("Authorized user not found");

            bool isGuestUser = await _context.Users.
                AnyAsync(u => u.Id == request.AuthUserId && u.UserRoles.Any(r => r.Role.Name == "Guest"), cancellationToken)
                && request.AuthUserRoles.Contains("Guest");

            if (isGuestUser) 
            { 
                throw new UnauthorizedException("Cannot delete a Guest user account");
            }

            bool isAdmin = await _context.Users.
                AnyAsync(u => u.Id == request.AuthUserId && u.UserRoles.Any(r => r.Role.Name == "Admin"), cancellationToken)
                && request.AuthUserRoles.Contains("Admin");

            if (isAdmin) 
            {
                throw new UnauthorizedException("An Administrator account cannot be deleted");
            }

            var userConnections = _context.Connections.Where(c => c.Username == user.UserName).ToList();
            _context.Connections.RemoveRange(userConnections);

            var posts = _context.Posts.Where(p => p.AppUserId == request.AuthUserId).ToList();
            _context.Posts.RemoveRange(posts);

            var userLikes = _context.Likes.Where(l => l.AppUserId == request.AuthUserId).ToList();
            foreach (var like in userLikes)
            {
                var post = _context.Posts.FirstOrDefault(p => p.PostId == like.PostId);
                if (post != null)
                {
                    post.LikesCount -= 1;
                }
            }

            var sentRequests = _context.FriendRequests.Where(fr => fr.SenderId == request.AuthUserId).ToList();
            var receivedRequests = _context.FriendRequests.Where(fr => fr.ReceiverId == request.AuthUserId).ToList();
            _context.FriendRequests.RemoveRange(sentRequests.Concat(receivedRequests));

            var friendshipsAsUser1 = _context.Friendships.Where(f => f.User1Id == request.AuthUserId).ToList();
            var friendshipsAsUser2 = _context.Friendships.Where(f => f.User2Id == request.AuthUserId).ToList();
            _context.Friendships.RemoveRange(friendshipsAsUser1.Concat(friendshipsAsUser2));

            var sentMessages = _context.PrivateMessages.Where(msg => msg.SenderId == request.AuthUserId).ToList();
            _context.PrivateMessages.RemoveRange(sentMessages);

            var receivedMessages = _context.PrivateMessages.Where(msg => msg.RecipientId == request.AuthUserId).ToList();
            _context.PrivateMessages.RemoveRange(receivedMessages);

            _context.Users.Remove(user);

            await _context.SaveChangesAsync(cancellationToken);

            _cacheInvalidationService.InvalidateSearchUserCache();
            _cacheInvalidationService.InvalidateNearestBirthdayUsersCache();
    
            return new DeleteUserResponse { IsDeleted = true };
        }
    }
}
