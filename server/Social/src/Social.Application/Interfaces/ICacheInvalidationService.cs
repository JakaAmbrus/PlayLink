namespace Social.Application.Interfaces
{
    public interface ICacheInvalidationService
    {
        void InvalidateUserCache(string username);
        void InvalidateSearchUserCache();
        void InvalidateFriendRequestsCache(int userId);
        void InvalidateUserFriendsCache(int userId);
        void InvalidateFriendshipStatusCache(int userId1, int userId2);
        void InvalidateUserPhotosCache(string username);
        void InvalidateNearestBirthdayUsersCache();
    }
}
