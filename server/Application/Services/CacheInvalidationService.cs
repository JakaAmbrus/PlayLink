using Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class CacheInvalidationService : ICacheInvalidationService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICacheKeyService _cacheKeyService;

        public CacheInvalidationService(IMemoryCache memoryCache, ICacheKeyService cacheKeyService)
        {
            _memoryCache = memoryCache;
            _cacheKeyService = cacheKeyService;
        }

        public void InvalidateFriendRequestsCache(int userId)
        {
            string cacheKey = _cacheKeyService.GenerateHashedKey($"Friends:GetFriendRequests-{userId}");
            _memoryCache.Remove(cacheKey);
        }
    }
}
