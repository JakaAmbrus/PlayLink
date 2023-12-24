using Application.Exceptions;
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

        public void InvalidateUserCache(string username)
        {
            try
            {
                string cacheKey = $"Users:GetUserByUsername-{username}";
                _memoryCache.Remove(cacheKey);
            }
            catch
            {
                throw new ServerErrorException("Could not invalidate user cache.");
            }
        }

        public void InvalidateSearchUserCache()
        {
            try
            {
                string cacheKey = "Users:GetSearchUsers";
                _memoryCache.Remove(cacheKey);
            }
            catch
            {
                throw new ServerErrorException("Could not invalidate search user cache.");
            }
        }

        public void InvalidateNearestBirthdayUsersCache()
        {
            try
            {
                string cacheKey = "Users:GetNearestBirthdayUsers";
                _memoryCache.Remove(cacheKey);
            }
            catch
            {
                throw new ServerErrorException("Could not invalidate nearest birthdays cache.");
            }
        }

        public void InvalidateFriendRequestsCache(int userId)
        {
            try
            {
                string cacheKey = $"Friends:GetFriendRequests-{userId}";
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
                string cacheKey = $"Friends:GetUserFriends-{userId}";
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

        public void InvalidateUserPhotosCache(string username)
        {
            try
            {
                string cacheKey = $"Photos:GetUserPhotos-{username}";
                _memoryCache.Remove(cacheKey);
            }
            catch
            {
                throw new ServerErrorException("Could not invalidate user photos cache.");
            }
        }
    }
}
