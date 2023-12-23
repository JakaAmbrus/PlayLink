using Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class CacheInvalidationService : ICacheInvalidationService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheInvalidationService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void InvalidateFriendRequestsCache(int userId)
        {
            string cacheKey = $"GetFriendRequests-{userId}";
            _memoryCache.Remove(cacheKey);
        }
    }
}
