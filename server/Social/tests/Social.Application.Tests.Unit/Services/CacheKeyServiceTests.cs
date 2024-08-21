using Social.Application.Interfaces;
using Social.Application.Services;

namespace Social.Application.Tests.Unit.Services
{
    public class CacheKeyServiceTests
    {
        private readonly ICacheKeyService _cacheKeyService;

        public CacheKeyServiceTests()
        {
            _cacheKeyService = new CacheKeyService();
        }

        [Theory]
        [InlineData(1, 2, "Friends:GetFriendStatus-1-2")]
        [InlineData(2, 1, "Friends:GetFriendStatus-1-2")]
        public void GenerateFriendStatusCacheKey_ShouldReturnCorrectKey_WhenThereAreDifferentIds(int userId1, int userId2, string expectedKey)
        {
            // Act
            var key = _cacheKeyService.GenerateFriendStatusCacheKey(userId1, userId2);

            // Assert
            key.Should().Be(expectedKey);
        }

        [Fact]
        public void GenerateFriendStatusCacheKey_ShouldThrowArgumentException_WhenBothIdsAreTheSame()
        {
            // Arrange
            int userId = 1;

            // Act
            var action = () => _cacheKeyService.GenerateFriendStatusCacheKey(userId, userId);

            // Assert
            action.Should().Throw<ArgumentException>()
                  .WithMessage("User IDs must be different when generating friend status cache key.");
        }
    }
}
