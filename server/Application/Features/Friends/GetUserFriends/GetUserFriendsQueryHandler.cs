using Application.Features.Friends.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Friends.GetUserFriends
{
    public class GetUserFriendsQueryHandler : IRequestHandler<GetUserFriendsQuery, GetUserFriendsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public GetUserFriendsQueryHandler(IApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<GetUserFriendsResponse> Handle(GetUserFriendsQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetUserFriends-{request.AuthUserId}";

            if (!_memoryCache.TryGetValue(cacheKey, out List<FriendDto> friends))
            {
                friends = await _context.Friendships
                   .Where(f => f.User1Id == request.AuthUserId || f.User2Id == request.AuthUserId)
                   .SelectMany(f => _context.Users
                   .Where(u => u.Id == (f.User1Id == request.AuthUserId ? f.User2Id : f.User1Id))
                   .Select(u => new FriendDto
                   {
                       Username = u.UserName,
                       FullName = u.FullName,
                       ProfilePictureUrl = u.ProfilePictureUrl,
                       Gender = u.Gender,
                       DateEstablished = f.DateEstablished
                   }))
                   .Distinct()
                   .ToListAsync(cancellationToken);

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _memoryCache.Set(cacheKey, friends, cacheEntryOptions);
            }
              
            return new GetUserFriendsResponse
            {
                Friends = friends
            };
        }
    }
}
