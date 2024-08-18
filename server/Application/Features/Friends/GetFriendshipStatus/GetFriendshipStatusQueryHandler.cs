using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Friends.GetRelationshipStatus
{
    public class GetFriendshipStatusQueryHandler : IRequestHandler<GetFriendshipStatusQuery, GetFriendshipStatusResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly ICacheKeyService _cacheKeyService;

        public GetFriendshipStatusQueryHandler(IApplicationDbContext context, IMemoryCache memoryCache, ICacheKeyService cacheKeyService)
        {
            _context = context;
            _memoryCache = memoryCache;
            _cacheKeyService = cacheKeyService;
        }

        public async Task<GetFriendshipStatusResponse> Handle(GetFriendshipStatusQuery request, CancellationToken cancellationToken)
        {            
            var profileUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.ProfileUsername, cancellationToken)
                ?? throw new NotFoundException("Profile user not found");

            string cacheKey = _cacheKeyService.GenerateFriendStatusCacheKey(request.AuthUserId, profileUser.Id);

            if (_memoryCache.TryGetValue(cacheKey, out GetFriendshipStatusResponse cachedResponse))
            {
                return cachedResponse;
            }

            var areFriends = await _context.Friendships.AnyAsync(f =>
                (f.User1Id == request.AuthUserId && f.User2Id == profileUser.Id) ||
                (f.User1Id == profileUser.Id && f.User2Id == request.AuthUserId),
                cancellationToken);

            var friendRequest = await _context.FriendRequests.FirstOrDefaultAsync(fr =>
                (fr.SenderId == request.AuthUserId && fr.ReceiverId == profileUser.Id) ||
                (fr.SenderId == profileUser.Id && fr.ReceiverId == request.AuthUserId),
                cancellationToken);

            var response = new GetFriendshipStatusResponse
            {
                Status = DetermineFriendshipStatus(areFriends, friendRequest)
            };

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
            _memoryCache.Set(cacheKey, response, cacheEntryOptions);

            return response;
        }

        private static FriendshipStatus DetermineFriendshipStatus(bool areFriends, FriendRequest friendRequest)
        {
            if (areFriends)
            {
                return FriendshipStatus.Friends;
            }
            if (friendRequest != null)
            {
                return friendRequest.Status switch
                {
                    FriendRequestStatus.Pending => FriendshipStatus.Pending,
                    FriendRequestStatus.Declined => FriendshipStatus.Declined,
                    _ => FriendshipStatus.None
                };
            }
            return FriendshipStatus.None;
        }
    }
}
