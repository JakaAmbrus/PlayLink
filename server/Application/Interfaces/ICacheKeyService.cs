namespace Application.Interfaces
{
    public interface ICacheKeyService
    {
        string GenerateHashedKey(string key);
        string GenerateFriendStatusCacheKey(int userId1, int userId2);
    }
}
