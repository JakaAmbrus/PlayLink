using Shield.Api.Common.Configurations;

namespace Shield.Api.Common.Abstractions;

public interface ICacheService
{
    GuestAccount GetLeastRecentlyUsedGuest(string role, List<GuestAccount> guestAccounts);
    
    void UpdateLastUsedGuest(string role, string email);
}