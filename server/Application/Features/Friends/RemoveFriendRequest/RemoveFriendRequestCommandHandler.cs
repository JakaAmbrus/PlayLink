using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Friends.RemoveFriendRequest
{
    public class RemoveFriendRequestCommandHandler : IRequestHandler<RemoveFriendRequestCommand, RemoveFriendRequestResponse>
    {
        private readonly IApplicationDbContext _context;

        public RemoveFriendRequestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RemoveFriendRequestResponse> Handle(RemoveFriendRequestCommand request, CancellationToken cancellationToken)
        {
            var authUser = await _context.Users
                .FindAsync(new object[] { request.AuthUserId }, cancellationToken)
                ?? throw new NotFoundException("User not found");

            var friendRequest = await _context.FriendRequests
                .FirstOrDefaultAsync(x => x.FriendRequestId == request.FriendRequestId && x.SenderId == request.AuthUserId, cancellationToken) 
                ?? throw new NotFoundException("Friend request not found or unauthorized access.");

            _context.FriendRequests.Remove(friendRequest);

            await _context.SaveChangesAsync(cancellationToken);

            return new RemoveFriendRequestResponse
            {
                RequestRemoved = true
            };
        }
    }
}
