﻿using Application.Exceptions;
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
            try
            {
                string cacheKey = _cacheKeyService.GenerateHashedKey($"Friends:GetFriendRequests-{userId}");
                _memoryCache.Remove(cacheKey);
            }
            catch
            {
                throw new ServerErrorException("Could not invalidate friend requests cache.");
            }
        }

        public void InvalidateUserFriendsCache(int userId)
        {
            try
            {
                string cacheKey = _cacheKeyService.GenerateHashedKey($"Friends:GetUserFriends-{userId}");
                _memoryCache.Remove(cacheKey);
            }
            catch
            {
                throw new ServerErrorException("Could not invalidate user friends cache.");
            }
        }

        public void InvalidateFriendshipStatusCache(int userId1, int userId2)
        {
            try
            {
                string cacheKey = _cacheKeyService.GenerateFriendStatusCacheKey(userId1, userId2);
                _memoryCache.Remove(cacheKey);
            }
            catch
            {
                throw new ServerErrorException("Could not invalidate friendship status cache.");
            }
        }
    }
}
