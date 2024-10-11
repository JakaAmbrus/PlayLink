using Social.Application.Features.Friends.RemoveFriendship;
using Social.Application.Interfaces;
using Social.Domain.Entities;
using Social.Domain.Enums;
using Social.Domain.Exceptions;
using MediatR;
using Social.UnitTests.Configurations;

namespace Social.UnitTests.Features.Friends
{
    public class RemoveFriendshipCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public RemoveFriendshipCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            _cacheInvalidationService = Substitute.For<ICacheInvalidationService>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<RemoveFriendshipCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new RemoveFriendshipCommandHandler(_context, _cacheInvalidationService)
                .Handle(c.Arg<RemoveFriendshipCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            var users = Enumerable.Range(1, 3)
                .Select(i => new AppUser
                {
                    Id = i,
                    UserName = $"{i}"
                }).ToList();
            context.Users.AddRange(users);

            context.Friendships.Add(new Friendship { User1Id = 1, User2Id = 2 });
            context.Friendships.Add(new Friendship { User1Id = 1, User2Id = 3 });

            context.FriendRequests.Add(new FriendRequest
            {
                FriendRequestId = 1,
                SenderId = 1,
                ReceiverId = 3,
                Status = FriendRequestStatus.Pending
            });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task RemoveFriendship_ShouldRemoveFriendship_WhenFriendshipExists()
        {
            // Arrange
            var request = new RemoveFriendshipCommand { AuthUserId = 1, ProfileUsername = "2" };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.FriendshipRemoved.Should().BeTrue();
            _context.Friendships.Should().HaveCount(1);      
        }

        [Fact]
        public async Task RemoveFriendship_ShouldRemoveFriendshipAndFriendRequest_WhenFriendshipAndFriendRequestExist()
        {
            // Arrange
            var request = new RemoveFriendshipCommand { AuthUserId = 1, ProfileUsername = "3" };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.FriendshipRemoved.Should().BeTrue();
            _context.Friendships.Should().HaveCount(1);
            _context.FriendRequests.Should().HaveCount(0);
        }

        [Fact]
        public async Task RemoveFriendship_ShouldInvalidateCaches_WhenFriendshipExists()
        {
            // Arrange
            var request = new RemoveFriendshipCommand { AuthUserId = 1, ProfileUsername = "2" };

            // Act
            await _mediator.Send(request);

            // Assert
            _cacheInvalidationService.Received(1).InvalidateUserFriendsCache(1);
            _cacheInvalidationService.Received(1).InvalidateUserFriendsCache(2);
            _cacheInvalidationService.Received(1).InvalidateFriendshipStatusCache(1, 2);
        }

        [Fact]
        public async Task RemoveFriendship_ShouldInvalidateCaches_WhenFriendshipAndFriendRequestExist()
        {
            // Arrange
            var request = new RemoveFriendshipCommand { AuthUserId = 1, ProfileUsername = "3" };

            // Act
            await _mediator.Send(request);

            // Assert
            _cacheInvalidationService.Received(1).InvalidateFriendRequestsCache(1);
        }

        [Fact]
        public async Task RemoveFriendship_ShouldThrowNotFoundException_WhenProfileUserDoesNotExist()
        {
            // Arrange
            var request = new RemoveFriendshipCommand { AuthUserId = 1, ProfileUsername = "4" };

            // Act
            Func<Task> act = async () => await _mediator.Send(request);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Profile user not found");
        }

        [Fact]
        public async Task RemoveFriendship_ShouldThrowNotFoundException_WhenFriendshipDoesNotExist()
        {
            // Arrange
            var request = new RemoveFriendshipCommand { AuthUserId = 2, ProfileUsername = "3" };

            // Act
            Func<Task> act = async () => await _mediator.Send(request);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Friendship not found");
        }
    }
}
