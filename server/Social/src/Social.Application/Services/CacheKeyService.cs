using Social.Application.Interfaces;

namespace Social.Application.Services
{
    public class CacheKeyService : ICacheKeyService
    {
        public string GenerateFriendStatusCacheKey(int userId1, int userId2)
        {
            if (userId1 == userId2)
            {
                throw new ArgumentException("User IDs must be different when generating friend status cache key.");
            }

            var organizedIds = (userId1 < userId2) ? (userId1, userId2) : (userId2, userId1);

            var baseKey = $"Friends:GetFriendStatus-{organizedIds.Item1}-{organizedIds.Item2}";

            return baseKey;
        }
    }
}
