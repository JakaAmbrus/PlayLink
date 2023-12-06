using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Friends.RemoveFriendship
{
    public class RemoveFriendshipCommandHandler : IRequestHandler<RemoveFriendshipCommand, RemoveFriendshipResponse>
    {
        private readonly IApplicationDbContext _context;

        public RemoveFriendshipCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RemoveFriendshipResponse> Handle(RemoveFriendshipCommand request, CancellationToken cancellationToken)
        {
            var AuthUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.AuthUserId, cancellationToken)
                ?? throw new NotFoundException("Auth user not found");

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
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new RemoveFriendshipResponse
            {
                FriendshipRemoved = true
            };
        }
    }
}
