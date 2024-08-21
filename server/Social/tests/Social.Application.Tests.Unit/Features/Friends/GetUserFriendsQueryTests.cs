using Social.Application.Features.Friends.Common;
using Social.Application.Features.Friends.GetUserFriends;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Social.Application.Tests.Unit.Features.Friends
{
    public class GetUserFriendsQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public GetUserFriendsQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            _memoryCache = Substitute.For<IMemoryCache>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetUserFriendsQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetUserFriendsQueryHandler(_context, _memoryCache)
                .Handle(c.Arg<GetUserFriendsQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            var users = Enumerable.Range(1, 5)
                .Select(i => new AppUser { Id = i, }).ToList();
            context.Users.AddRange(users);

            var friendships = Enumerable.Range(2, 3)
                .Select(i => new Friendship
                {
                    User1Id = i,
                    User2Id = 1,
                    DateEstablished = DateTime.UtcNow.AddDays(-i)
                }).ToList();
            context.Friendships.AddRange(friendships);

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetUserFriends_ShouldReturnUsersFriends_WhenUserHasFriendsAndThereIsNoCache()
        {
            // Arrange
            var request = new GetUserFriendsQuery { AuthUserId = 1 };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.Friends.Should().AllBeOfType<FriendDto>();
            response.Friends.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetUserFriends_ShouldReturnUsersFriends_WhenUserHasFriendsAndThereIsCache()
        {
            // Arrange
            var request = new GetUserFriendsQuery { AuthUserId = 1 };
            var cacheKey = $"Friends:GetUserFriends-{request.AuthUserId}";
            var cachedResponse = new GetUserFriendsResponse
            {
                Friends = new List<FriendDto>
                {
                    new FriendDto { Username = "2" },
                    new FriendDto { Username = "3" },
                    new FriendDto { Username = "4" }
                }
            };
            _memoryCache.TryGetValue(cacheKey, out Arg.Any<GetUserFriendsResponse>()).Returns(x =>
            {
                x[1] = cachedResponse;
                return true;
            });

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.Friends.Should().AllBeOfType<FriendDto>();
            response.Friends.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetUserFriends_ShouldReturnEmptyList_WhenUserHasNoFriendsAndThereIsNoCache()
        {
            // Arrange
            var request = new GetUserFriendsQuery { AuthUserId = 5 };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.Friends.Should().BeEmpty();
        }

        [Fact]
        public async Task GetUserFriends_ShouldReturnEmptyList_WhenUserHasNoFriendsAndThereIsCache()
        {
            // Arrange
            var request = new GetUserFriendsQuery { AuthUserId = 5 };
            var cacheKey = $"Friends:GetUserFriends-{request.AuthUserId}";
            var cachedResponse = new GetUserFriendsResponse { Friends = new List<FriendDto> { } };
            _memoryCache.TryGetValue(cacheKey, out Arg.Any<GetUserFriendsResponse>()).Returns(x =>
            {
                x[1] = cachedResponse;
                return true;
            });

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.Friends.Should().BeEmpty();
        }
    }
}
