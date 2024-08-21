using Social.Domain.Entities;
using Social.Domain.Enums;
using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Interfaces;

namespace Social.Application.Features.Friends.SendFriendRequest
{
    public class SendFriendRequestCommandHandler : IRequestHandler<SendFriendRequestCommand, SendFriendRequestResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public SendFriendRequestCommandHandler(IApplicationDbContext context, ICacheInvalidationService cacheInvalidationService)
        {
            _context = context;
            _cacheInvalidationService = cacheInvalidationService;
        }

        public async Task<SendFriendRequestResponse> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
        {
            var receiver = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == request.ReceiverUsername, cancellationToken)
                ?? throw new NotFoundException("Receiver not found");

            bool existingFriendship = await _context.Friendships
                .AnyAsync(f => (f.User1Id == request.AuthUserId && f.User2Id == receiver.Id) || (f.User1Id == receiver.Id && f.User2Id == request.AuthUserId), cancellationToken);

            if (existingFriendship)
            {
                throw new BadRequestException("You are already friends with this user");
            }

            if (receiver.Id == request.AuthUserId)
            {
                throw new BadRequestException("You cannot send a friend request to yourself");
            }

            var friendRequest = await _context.FriendRequests
                .FirstOrDefaultAsync(x => x.SenderId == request.AuthUserId && x.ReceiverId == receiver.Id, cancellationToken);

            if (friendRequest != null)
            {
                if (friendRequest.Status == FriendRequestStatus.Declined)
                {
                    throw new BadRequestException("A previous friend request was declined, wait for the owner to get notified before resending");
                }

                throw new BadRequestException("Friend request already sent");
            }

            friendRequest = new FriendRequest
            {
                SenderId = request.AuthUserId,
                ReceiverId = receiver.Id,
                Status = FriendRequestStatus.Pending
            };

            await _context.FriendRequests.AddAsync(friendRequest, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            _cacheInvalidationService.InvalidateFriendRequestsCache(friendRequest.ReceiverId);
            _cacheInvalidationService.InvalidateFriendshipStatusCache(request.AuthUserId, receiver.Id);

            return new SendFriendRequestResponse { RequestSent = true };
        }
    }
}
