namespace Application.Interfaces
{
    public interface ICacheInvalidationService
    {
        void InvalidateFriendRequestsCache(int userId);
    }
}
