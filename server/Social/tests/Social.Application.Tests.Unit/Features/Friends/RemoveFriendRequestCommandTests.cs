using Social.Application.Features.Friends.RemoveFriendRequest;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Domain.Entities;
using Social.Domain.Enums;
using Social.Domain.Exceptions;
using MediatR;

namespace Social.Application.Tests.Unit.Features.Friends
{
    public class RemoveFriendRequestCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public RemoveFriendRequestCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            _cacheInvalidationService = Substitute.For<ICacheInvalidationService>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<RemoveFriendRequestCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new RemoveFriendRequestCommandHandler(_context, _cacheInvalidationService)
                               .Handle(c.Arg<RemoveFriendRequestCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            var users = Enumerable.Range(1, 4)
                .Select(i => new AppUser
                {
                    Id = i,
                    UserName = $"{i}",
                    FullName = $"{i} Tester",
                }).ToList();
            context.Users.AddRange(users);

            context.FriendRequests.Add(new FriendRequest
            {
                FriendRequestId = 1,
                SenderId = 1,
                ReceiverId = 3,
                Status = FriendRequestStatus.Pending
            });
            context.FriendRequests.Add(new FriendRequest
            {
                FriendRequestId = 2,
                SenderId = 2,
                ReceiverId = 3,
                Status = FriendRequestStatus.Declined
            });
            context.FriendRequests.Add(new FriendRequest
            {
                FriendRequestId = 3,
                SenderId = 3,
                ReceiverId = 4,
                Status = FriendRequestStatus.Accepted
            });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        public async Task RemoveFriendRequest_ShouldRemoveFriendRequest_WhenFriendRequestExists(
            int userId, int requestId)
        {
            // Arrange
            var request = new RemoveFriendRequestCommand { AuthUserId = userId, FriendRequestId = requestId };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.RequestRemoved.Should().BeTrue();
            _context.FriendRequests.Should().HaveCount(2);
            _context.FriendRequests.Should().NotContain(x => x.FriendRequestId == requestId);
        }

        [Fact]
        public async Task RemoveFriendRequest_ShouldInvalidateCache_WhenFriendRequestIsDeclined()
        {
            // Arrange
            var request = new RemoveFriendRequestCommand { AuthUserId = 2, FriendRequestId = 2 };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.RequestRemoved.Should().BeTrue();
            _cacheInvalidationService.Received(1).InvalidateFriendRequestsCache(request.AuthUserId);
            _cacheInvalidationService.Received(1).InvalidateFriendshipStatusCache(2, 3);
        }

        [Fact]
        public async Task RemoveFriendRequest_ShouldThrowNotFoundException_WhenFriendRequestDoesNotExist()
        {
            // Arrange
            var request = new RemoveFriendRequestCommand { AuthUserId = 1, FriendRequestId = 4 };

            // Act
            Func<Task> action = async () => { await _mediator.Send(request); };

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Friend request not found or unauthorized access.");
        }
    }
}
