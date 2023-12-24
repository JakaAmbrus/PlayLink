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
    }
}
