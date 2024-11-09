using Microsoft.Extensions.Caching.Memory;
using Shield.Api.Common.Abstractions;
using Shield.Api.Common.Configurations;


namespace Shield.Api.Common.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    private const string GuestCacheKeyPrefix = "GuestLastUsed_";
    
    public GuestAccount GetLeastRecentlyUsedGuest(string role, List<GuestAccount> guestAccounts)
    {
        var leastRecentlyUsedGuest = guestAccounts
            .OrderBy(guest => _cache.TryGetValue(GetGuestCacheKey(role, guest.Email), out DateTime lastUsed) ? lastUsed : DateTime.MinValue)
            .First();

        return leastRecentlyUsedGuest;
    }
    
    public void UpdateLastUsedGuest(string role, string email)
    {
        _cache.Set(GetGuestCacheKey(role, email), DateTime.UtcNow);
    }
    
    private static string GetGuestCacheKey(string role, string email)
    {
        return $"{GuestCacheKeyPrefix}{role}_{email}";
    }
}