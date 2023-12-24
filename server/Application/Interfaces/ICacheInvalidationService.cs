namespace Application.Interfaces
{
    public interface ICacheInvalidationService
    {
        void InvalidateFriendRequestsCache(int userId);
        void InvalidateUserFriendsCache(int userId);
        void InvalidateFriendshipStatusCache(int userId1, int userId2);
        void InvalidateUserPhotosCache(string username);
    }
}
