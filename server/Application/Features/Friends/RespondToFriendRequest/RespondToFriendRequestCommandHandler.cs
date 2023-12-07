using Application.Exceptions;
using Application.Features.Friends.Common;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Friends.RespondToFriendRequest
{
    public class RespondToFriendRequestCommandHandler : IRequestHandler<RespondToFriendRequestCommand, RespondToFriendRequestResponse>
    {
        private readonly IApplicationDbContext _context;

        public RespondToFriendRequestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RespondToFriendRequestResponse> Handle(RespondToFriendRequestCommand request, CancellationToken cancellationToken)
        {
            var friendRequest = await _context.FriendRequests
                .FindAsync(new object[] { request.FriendRequestResponse.FriendRequestId }, cancellationToken)
                ?? throw new NotFoundException("Friend request not found");

            if (friendRequest.ReceiverId != request.AuthUserId)
            {
                throw new UnauthorizedException("You are not authorized to respond to this friend request");
            }

            if (friendRequest.Status != FriendRequestStatus.Pending)
            {
                throw new InvalidOperationException("The friend request is not in a pending state.");
            }

            if (request.FriendRequestResponse.Accept)
            {
                var friend = new Friendship
                {
                    User1Id = friendRequest.SenderId,
                    User2Id = friendRequest.ReceiverId,
                };

                friendRequest.Status = FriendRequestStatus.Accepted;

                await _context.Friendships.AddAsync(friend, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var newFriend = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == friendRequest.SenderId, cancellationToken)
                    ?? throw new NotFoundException("User not found");

                return new RespondToFriendRequestResponse
                {
                    RequestAccepted = true,
                    FriendDto = new FriendDto
                    {
                        Username = newFriend.UserName,
                        FullName = newFriend.FullName,
                        ProfilePictureUrl = newFriend.ProfilePictureUrl,
                        Gender = newFriend.Gender,
                        DateEstablished = DateTime.UtcNow
                    }

                };
            }

            friendRequest.Status = FriendRequestStatus.Declined;
            await _context.SaveChangesAsync(cancellationToken);

            return new RespondToFriendRequestResponse
            {
                RequestAccepted = false
            };         
        }
    }
}
