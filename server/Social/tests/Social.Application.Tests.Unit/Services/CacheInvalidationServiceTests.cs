using Social.Application.Interfaces;
using Social.Application.Services;
using Social.Domain.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace Social.Application.Tests.Unit.Services
{
    public class CacheInvalidationServiceTests
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICacheKeyService _cacheKeyService;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public CacheInvalidationServiceTests()
        {
            _memoryCache = Substitute.For<IMemoryCache>();
            _cacheKeyService = Substitute.For<ICacheKeyService>();
            _cacheInvalidationService = new CacheInvalidationService(_memoryCache, _cacheKeyService);
        }

        [Fact]
        public void InvalidateUserCache_ShouldInvalidateCorrectCacheKey_WhenCalled()
        {
            // Arrange
            string username = "Tester";

            // Act
            _cacheInvalidationService.InvalidateUserCache(username);

            // Assert
            _memoryCache.Received(1).Remove($"Users:GetUserByUsername-{username}");
        }

        [Fact]
        public void InvalidateUserCache_ShouldThrowServerErrorException_WhenThereIsAnError()
        {
            // Arrange
            string username = "Tester";
            _memoryCache.When(x => x.Remove(Arg.Any<string>())).Do(x => { throw new Exception(); });

            // Act
            var action = () => _cacheInvalidationService.InvalidateUserCache(username);

            // Assert
            action.Should().Throw<ServerErrorException>()
                .WithMessage("Could not invalidate user cache.");
        }

        [Fact]
        public void InvalidateSearchUserCache_ShouldInvalidateCorrectCacheKey_WhenCalled()
        {
            // Act
            _cacheInvalidationService.InvalidateSearchUserCache();

            // Assert
            _memoryCache.Received(1).Remove("Users:GetSearchUsers");
        }

        [Fact]
        public void InvalidateSearchUserCache_ShouldThrowServerErrorException_WhenThereIsAnError()
        {
            // Arrange
            _memoryCache.When(x => x.Remove(Arg.Any<string>())).Do(x => { throw new Exception(); });

            // Act
            var action = () => _cacheInvalidationService.InvalidateSearchUserCache();

            // Assert
            action.Should().Throw<ServerErrorException>()
                .WithMessage("Could not invalidate search user cache.");
        }

        [Fact]
        public void InvalidateNearestBirthdayUsersCache_ShouldInvalidateCorrectCacheKey_WhenCalled()
        {
            // Act
            _cacheInvalidationService.InvalidateNearestBirthdayUsersCache();

            // Assert
            _memoryCache.Received(1).Remove("Users:GetNearestBirthdayUsers");
        }

        [Fact]
        public void InvalidateNearestBirthdayUsersCache_ShouldThrowServerErrorException_WhenThereIsAnError()
        {
            // Arrange
            _memoryCache.When(x => x.Remove(Arg.Any<string>())).Do(x => { throw new Exception(); });

            // Act
            var action = () => _cacheInvalidationService.InvalidateNearestBirthdayUsersCache();

            // Assert
            action.Should().Throw<ServerErrorException>()
                .WithMessage("Could not invalidate nearest birthdays cache.");
        }

        [Fact]
        public void InvalidateFriendRequestsCache_ShouldInvalidateCorrectCacheKey_WhenCalled()
        {
            // Arrange
            int userId = 123;

            // Act
            _cacheInvalidationService.InvalidateFriendRequestsCache(userId);

            // Assert
            _memoryCache.Received(1).Remove($"Friends:GetFriendRequests-{userId}");
        }

        [Fact]
        public void InvalidateFriendRequestsCache_ShouldThrowServerErrorException_WhenThereIsAnError()
        {
            // Arrange
            int userId = 123;
            _memoryCache.When(x => x.Remove(Arg.Any<string>())).Do(x => { throw new Exception(); });

            // Act
            var action = () => _cacheInvalidationService.InvalidateFriendRequestsCache(userId);

            // Assert
            action.Should().Throw<ServerErrorException>()
                .WithMessage("Could not invalidate friend requests cache.");
        }

        [Fact]
        public void InvalidateUserPhotosCache_ShouldInvalidateCorrectCacheKey_WhenCalled()
        {
            // Arrange
            string username = "Tester";

            // Act
            _cacheInvalidationService.InvalidateUserPhotosCache(username);

            // Assert
            _memoryCache.Received(1).Remove($"Photos:GetUserPhotos-{username}");
        }

        [Fact]
        public void InvalidateUserPhotosCache_ShouldThrowServerErrorException_WhenThereIsAnError()
        {
            // Arrange
            string username = "Tester";
            _memoryCache.When(x => x.Remove(Arg.Any<string>())).Do(x => { throw new Exception(); });

            // Act
            var action = () => _cacheInvalidationService.InvalidateUserPhotosCache(username);

            // Assert
            action.Should().Throw<ServerErrorException>()
                .WithMessage("Could not invalidate user photos cache.");
        }
    }
}
