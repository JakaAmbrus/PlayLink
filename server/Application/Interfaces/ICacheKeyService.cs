namespace Application.Interfaces
{
    public interface ICacheKeyService
    {
        string GenerateFriendStatusCacheKey(int userId1, int userId2);
    }
}
