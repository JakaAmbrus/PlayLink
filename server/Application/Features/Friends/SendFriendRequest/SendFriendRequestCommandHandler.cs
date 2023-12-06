using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Friends.SendFriendRequest
{
    public class SendFriendRequestCommandHandler : IRequestHandler<SendFriendRequestCommand, SendFriendRequestResponse>
    {
        private readonly IApplicationDbContext _context;

        public SendFriendRequestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SendFriendRequestResponse> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FindAsync(new object[] { request.AuthUserId }, cancellationToken)
                ?? throw new NotFoundException("User not found");

            var receiver = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == request.ReceiverUsername, cancellationToken)
                ?? throw new NotFoundException("Receiver not found");

            bool existingFriendship = await _context.Friendships
                .AnyAsync(f => (f.User1Id == user.Id && f.User2Id == receiver.Id) || (f.User1Id == receiver.Id && f.User2Id == user.Id), cancellationToken);

            if (existingFriendship)
            {
                throw new BadRequestException("You are already friends with this user.");
            }

            if (receiver.Id == request.AuthUserId)
            {
                throw new BadRequestException("You cannot send a friend request to yourself");
            }

            var friendRequest = await _context.FriendRequests
                .FirstOrDefaultAsync(x => x.SenderId == user.Id && x.ReceiverId == receiver.Id, cancellationToken);

            if (friendRequest != null)
            {
                if (friendRequest.Status == FriendRequestStatus.Declined)
                {
                    throw new BadRequestException("A previous friend request was declined, wait for the owner to get notified before resending.");
                }

                throw new BadRequestException("Friend request already sent");
            }

            friendRequest = new FriendRequest
            {
                SenderId = user.Id,
                ReceiverId = receiver.Id,
                Status = FriendRequestStatus.Pending
            };

            await _context.FriendRequests.AddAsync(friendRequest, cancellationToken);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            return new SendFriendRequestResponse { RequestSent = success };
        }
    }
}
