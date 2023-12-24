using Application.Interfaces;

namespace Application.Services
{
    public class CacheKeyService : ICacheKeyService
    {
        public string GenerateFriendStatusCacheKey(int userId1, int userId2)
        {
            var organizedIds = (userId1 < userId2) ? (userId1, userId2) : (userId2, userId1);


            var baseKey = $"Friends:GetFriendStatus-{organizedIds.Item1}-{organizedIds.Item2}";

            return baseKey;
        }
    }
}
