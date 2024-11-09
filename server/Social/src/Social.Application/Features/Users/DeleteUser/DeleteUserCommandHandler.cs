using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Interfaces;

namespace Social.Application.Features.Users.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
    {
        private readonly ISocialDbContext _context;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public DeleteUserCommandHandler(ISocialDbContext context, ICacheInvalidationService cacheInvalidationService)
        {
            _context = context;
            _cacheInvalidationService = cacheInvalidationService;
        }

        public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken)
                ?? throw new NotFoundException("User not found");
            
            if (request.SignUpFailure != true)
            {
                var userConnections = _context.Connections.Where(c => c.Username == user.Username).ToList();
                _context.Connections.RemoveRange(userConnections);

                var posts = _context.Posts.Where(p => p.AppUserId == request.UserId).ToList();
                _context.Posts.RemoveRange(posts);

                var userLikes = _context.Likes.Where(l => l.AppUserId == request.UserId).ToList();
                foreach (var like in userLikes)
                {
                    var post = _context.Posts.FirstOrDefault(p => p.PostId == like.PostId);
                    if (post != null)
                    {
                        post.LikesCount -= 1;
                    }
                }

                var sentRequests = _context.FriendRequests.Where(fr => fr.SenderId == request.UserId).ToList();
                var receivedRequests = _context.FriendRequests.Where(fr => fr.ReceiverId == request.UserId).ToList();
                _context.FriendRequests.RemoveRange(sentRequests.Concat(receivedRequests));

                var friendshipsAsUser1 = _context.Friendships.Where(f => f.User1Id == request.UserId).ToList();
                var friendshipsAsUser2 = _context.Friendships.Where(f => f.User2Id == request.UserId).ToList();
                _context.Friendships.RemoveRange(friendshipsAsUser1.Concat(friendshipsAsUser2));

                var sentMessages = _context.PrivateMessages.Where(msg => msg.SenderId == request.UserId).ToList();
                _context.PrivateMessages.RemoveRange(sentMessages);

                var receivedMessages = _context.PrivateMessages.Where(msg => msg.RecipientId == request.UserId).ToList();
                _context.PrivateMessages.RemoveRange(receivedMessages);
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync(cancellationToken);

            if (request.SignUpFailure != true)
            {
                _cacheInvalidationService.InvalidateSearchUserCache();
                _cacheInvalidationService.InvalidateNearestBirthdayUsersCache();
            }
    
            return new DeleteUserResponse();
        }
    }
}
