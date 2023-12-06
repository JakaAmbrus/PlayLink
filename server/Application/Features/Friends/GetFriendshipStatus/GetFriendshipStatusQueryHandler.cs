using Application.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Friends.GetRelationshipStatus
{
    public class GetFriendshipStatusQueryHandler : IRequestHandler<GetFriendshipStatusQuery, GetFriendshipStatusResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetFriendshipStatusQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetFriendshipStatusResponse> Handle(GetFriendshipStatusQuery request, CancellationToken cancellationToken)
        {
            var profileUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.ProfileUsername, cancellationToken)
                ?? throw new NotFoundException("User not found");

            var areFriends = await _context.Friendships.AnyAsync(f =>
                (f.User1Id == request.AuthUserId && f.User2Id == profileUser.Id) ||
                (f.User1Id == profileUser.Id && f.User2Id == request.AuthUserId),
                cancellationToken);

            if (areFriends)
            {
                return new GetFriendshipStatusResponse
                {
                    Status = FriendshipStatus.Friends
                };
            }

            var friendRequest = await _context.FriendRequests.FirstOrDefaultAsync(fr =>
                (fr.SenderId == request.AuthUserId && fr.ReceiverId == profileUser.Id) ||
                (fr.SenderId == profileUser.Id && fr.ReceiverId == request.AuthUserId),
                cancellationToken);

            if (friendRequest != null)
            {
                if (friendRequest.Status == FriendRequestStatus.Pending)
                {
                    return new GetFriendshipStatusResponse
                    {
                        Status = FriendshipStatus.Pending
                    };
                }
                else if (friendRequest.Status == FriendRequestStatus.Declined)
                {
                    return new GetFriendshipStatusResponse
                    {
                        Status = FriendshipStatus.Declined
                    };
                }
            }

            return new GetFriendshipStatusResponse
            {
                Status = FriendshipStatus.None
            };
        }
    }
}
