using Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class CacheKeyService : ICacheKeyService
    {
        private static readonly string Salt = "RandomSaltPlaceholder";

        public string GenerateHashedKey(string key)
        {
            var saltedKey = key + Salt;
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(saltedKey));

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public string GenerateFriendStatusCacheKey(int userId1, int userId2)
        {
            var organizedIds = (userId1 < userId2) ? (userId1, userId2) : (userId2, userId1);

            var baseKey = $"Friends:GetFriendStatus-{organizedIds.Item1}-{organizedIds.Item2}";

            return GenerateHashedKey(baseKey);
        }
    }
}
