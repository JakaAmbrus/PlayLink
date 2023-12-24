using Application.Features.Friends.Common;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Friends.GetFriendRequests
{
    public class GetFriendRequestsQueryHandler : IRequestHandler<GetFriendRequestsQuery, GetFriendRequestsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly ICacheKeyService _cacheKeyService;

        public GetFriendRequestsQueryHandler(IApplicationDbContext context, IMemoryCache memoryCache, ICacheKeyService cacheKeyService)
        {
            _context = context;
            _memoryCache = memoryCache;
            _cacheKeyService = cacheKeyService;
        }

        public async Task<GetFriendRequestsResponse> Handle(GetFriendRequestsQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = _cacheKeyService.GenerateHashedKey($"Friends:GetFriendRequests-{request.AuthUserId}");

            if (!_memoryCache.TryGetValue(cacheKey, out List<FriendRequestDto> allRequests))
            {
                var incomingRequests = await _context.FriendRequests
               .Include(fr => fr.Sender)
               .Where(fr => fr.ReceiverId == request.AuthUserId && fr.Status == FriendRequestStatus.Pending)
               .Select(fr => new FriendRequestDto
               {
                   FriendRequestId = fr.FriendRequestId,
                   SenderUsername = fr.Sender.UserName,
                   SenderFullName = fr.Sender.FullName,
                   SenderProfilePictureUrl = fr.Sender.ProfilePictureUrl,
                   SenderGender = fr.Sender.Gender,
                   Status = fr.Status,
                   TimeSent = fr.TimeSent
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

                allRequests = incomingRequests.Concat(sentRequestResponses).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _memoryCache.Set(cacheKey, allRequests, cacheEntryOptions);
            }

            return new GetFriendRequestsResponse
            {
                FriendRequests = allRequests
            };
        }   
    }
}
