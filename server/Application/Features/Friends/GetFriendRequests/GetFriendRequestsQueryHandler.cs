using Application.Features.Friends.Common;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Friends.GetFriendRequests
{
    public class GetFriendRequestsQueryHandler : IRequestHandler<GetFriendRequestsQuery, GetFriendRequestsResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetFriendRequestsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetFriendRequestsResponse> Handle(GetFriendRequestsQuery request, CancellationToken cancellationToken)
        {
            var incomingRequests = await _context.FriendRequests
                .Include(x => x.Sender)
                .Where(x => x.ReceiverId == request.AuthUserId && x.Status == FriendRequestStatus.Pending)
                .Select(x => new FriendRequestDto
                {
                    FriendRequestId = x.FriendRequestId,
                    SenderUsername = x.Sender.UserName,
                    SenderFullName = x.Sender.FullName,
                    SenderProfilePictureUrl = x.Sender.ProfilePictureUrl,
                    SenderGender = x.Sender.Gender,
                    Status = x.Status
                })
                .ToListAsync(cancellationToken);

            var sentRequestResponses = await _context.FriendRequests
            .Where(fr => fr.SenderId == request.AuthUserId && fr.Status != FriendRequestStatus.Pending)
                .Select(fr => new FriendRequestDto
                {
                    FriendRequestId = fr.FriendRequestId,
                    RecipientUsername = fr.Receiver.UserName,
                    RecipientFullName = fr.Receiver.FullName,
                    RecipientProfilePictureUrl = fr.Receiver.ProfilePictureUrl,
                    RecipientGender = fr.Receiver.Gender,
                    Status = fr.Status
                })
                .ToListAsync(cancellationToken);

            var allRequests = incomingRequests.Concat(sentRequestResponses).ToList();

            return new GetFriendRequestsResponse
            {
                FriendRequests = allRequests
            };
        }
    }
}
