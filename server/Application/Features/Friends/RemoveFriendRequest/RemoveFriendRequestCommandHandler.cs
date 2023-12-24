﻿using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Friends.RemoveFriendRequest
{
    public class RemoveFriendRequestCommandHandler : IRequestHandler<RemoveFriendRequestCommand, RemoveFriendRequestResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public RemoveFriendRequestCommandHandler(IApplicationDbContext context, ICacheInvalidationService cacheInvalidationService)
        {
            _context = context;
            _cacheInvalidationService = cacheInvalidationService;
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

            _cacheInvalidationService.InvalidateFriendRequestsCache(request.AuthUserId);

            if(friendRequest.Status == Domain.Enums.FriendRequestStatus.Declined)
            {
                _cacheInvalidationService.InvalidateFriendshipStatusCache(friendRequest.SenderId, friendRequest.ReceiverId);
            }

            return new RemoveFriendRequestResponse
            {
                RequestRemoved = true
            };
        }
    }
}
