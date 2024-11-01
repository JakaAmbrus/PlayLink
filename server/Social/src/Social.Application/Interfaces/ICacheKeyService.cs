namespace Social.Application.Interfaces
{
    public interface ICacheKeyService
    {
        string GenerateFriendStatusCacheKey(long userId1, long userId2);
    }
}
