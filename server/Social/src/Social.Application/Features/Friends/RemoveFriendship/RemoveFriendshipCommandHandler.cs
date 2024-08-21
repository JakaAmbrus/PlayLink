using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Interfaces;

namespace Social.Application.Features.Friends.RemoveFriendship
{
    public class RemoveFriendshipCommandHandler : IRequestHandler<RemoveFriendshipCommand, RemoveFriendshipResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public RemoveFriendshipCommandHandler(IApplicationDbContext context, ICacheInvalidationService cacheInvalidationService)
        {
            _context = context;
            _cacheInvalidationService = cacheInvalidationService;
        }

        public async Task<RemoveFriendshipResponse> Handle(RemoveFriendshipCommand request, CancellationToken cancellationToken)
        {
            var profileUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.ProfileUsername, cancellationToken)
                ?? throw new NotFoundException("Profile user not found");

            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f =>
                    (f.User1Id == request.AuthUserId && f.User2Id == profileUser.Id) ||
                    (f.User1Id == profileUser.Id && f.User2Id == request.AuthUserId),
                    cancellationToken)
                ?? throw new NotFoundException("Friendship not found");

            _context.Friendships.Remove(friendship);

            var friendRequest = await _context.FriendRequests
                .FirstOrDefaultAsync(fr =>
                    (fr.SenderId == request.AuthUserId && fr.ReceiverId == profileUser.Id) ||
                    (fr.SenderId == profileUser.Id && fr.ReceiverId == request.AuthUserId),
                    cancellationToken);

            if (friendRequest != null)
            {
                _context.FriendRequests.Remove(friendRequest);
                _cacheInvalidationService.InvalidateFriendRequestsCache(friendRequest.SenderId);
            }

            await _context.SaveChangesAsync(cancellationToken);

            _cacheInvalidationService.InvalidateUserFriendsCache(request.AuthUserId);
            _cacheInvalidationService.InvalidateUserFriendsCache(profileUser.Id);

            _cacheInvalidationService.InvalidateFriendshipStatusCache(request.AuthUserId, profileUser.Id);

            return new RemoveFriendshipResponse { FriendshipRemoved = true };
        }
    }
}
