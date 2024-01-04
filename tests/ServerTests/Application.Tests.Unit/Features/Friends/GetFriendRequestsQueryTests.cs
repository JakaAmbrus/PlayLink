using Application.Exceptions;
using Application.Features.Friends.Common;
using Application.Features.Friends.GetFriendRequests;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Tests.Unit.Features.Friends
{
    public class GetFriendRequestsQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public GetFriendRequestsQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            _memoryCache = Substitute.For<IMemoryCache>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetFriendRequestsQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetFriendRequestsQueryHandler(_context, _memoryCache)
                .Handle(c.Arg<GetFriendRequestsQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            var users = Enumerable.Range(1, 12)
                .Select(i => new AppUser
                {
                    Id = i,
                    UserName = $"{i}",
                    FullName = $"{i} Tester",
                }).ToList();
            context.Users.AddRange(users);

            var pendingFriendRequests = Enumerable.Range(1, 10)
                .Select(i => new FriendRequest
                {
                    SenderId = i,
                    ReceiverId = 11,
                    Status = FriendRequestStatus.Pending,
                }).ToList();
            context.FriendRequests.AddRange(pendingFriendRequests);

            var acceptedFriendRequests = Enumerable.Range(1, 5)
                .Select(i => new FriendRequest
                {
                    SenderId = 12,
                    ReceiverId = i,
                    Status = FriendRequestStatus.Accepted,
                }).ToList();
            context.FriendRequests.AddRange(acceptedFriendRequests);

            var declinedFriendRequests = Enumerable.Range(6, 5)
                .Select(i => new FriendRequest
                {
                    SenderId = 12,
                    ReceiverId = i,
                    Status = FriendRequestStatus.Declined,
                }).ToList();
            context.FriendRequests.AddRange(declinedFriendRequests);

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetFriendRequests_ShouldRetrievePendingFriendRequests_WhenThereIsNoCache()
        {

            // Arrange
            var request = new GetFriendRequestsQuery { AuthUserId = 11 };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.FriendRequests.Should().NotBeEmpty();
            response.FriendRequests.Should().AllBeOfType<FriendRequestDto>();
            response.FriendRequests.Count.Should().Be(10);
            response.FriendRequests[0].SenderFullName.Should().Be("1 Tester");
            response.FriendRequests[0].SenderUsername.Should().Be("1");
        }

        [Fact]
        public async Task GetFriendRequests_ShouldRetrievePendingFriendRequestsFromCache_WhenThereIsCache()
        {
            // Arrange
            var request = new GetFriendRequestsQuery { AuthUserId = 11 };
            var cachedFriendRequests = Enumerable.Range(1, 10)
                .Select(i => new FriendRequestDto
                {
                    FriendRequestId = i,
                    SenderUsername = $"{i}",
                    SenderFullName = $"{i} Tester",
                }).ToList();

            _memoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<List<FriendRequestDto>>())
              .Returns(x =>
              {
                  x[1] = cachedFriendRequests;
                  return true;
              });

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.FriendRequests.Should().BeEquivalentTo(cachedFriendRequests);
        }

        [Fact]
        public async Task GetFriendRequests_ShouldReturnEmptyList_WhenThereAreNoPendingFriendRequests()
        {
            // Arrange
            var request = new GetFriendRequestsQuery { AuthUserId = 1 };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.FriendRequests.Should().BeEmpty();
        }

        [Fact]
        public async Task GetFriendRequests_ShouldRetrieveNonPendingFriendRequests_WhenThereIsNoCache()
        {
            // Arrange
            var request = new GetFriendRequestsQuery { AuthUserId = 12 };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.FriendRequests.Should().NotBeEmpty();
            response.FriendRequests.Should().AllBeOfType<FriendRequestDto>();
            response.FriendRequests.Count.Should().Be(10);
            response.FriendRequests[0].RecipientFullName.Should().Be("1 Tester");
            response.FriendRequests[0].RecipientUsername.Should().Be("1");
        }

        [Fact]
        public async Task GetFriendRequests_ShouldRetrieveNonPendingFriendRequestsFromCache_WhenThereIsCache()
        {
            // Arrange
            var request = new GetFriendRequestsQuery { AuthUserId = 12 };
            var cachedFriendRequests = Enumerable.Range(1, 10)
                .Select(i => new FriendRequestDto
                {
                    FriendRequestId = i,
                    RecipientUsername = $"{i}",
                    RecipientFullName = $"{i} Tester",
                }).ToList();

            _memoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<List<FriendRequestDto>>())
              .Returns(x =>
              {
                  x[1] = cachedFriendRequests;
                  return true;
              });

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.FriendRequests.Should().BeEquivalentTo(cachedFriendRequests);
        }

        [Fact]
        public async Task GetFriendRequests_ShouldReturnEmptyList_WhenThereAreNoNonPendingFriendRequests()
        {
            // Arrange
            var request = new GetFriendRequestsQuery { AuthUserId = 2 };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.FriendRequests.Should().BeEmpty();
        }

        [Fact]
        public async Task GetFriendRequests_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new GetFriendRequestsQuery { AuthUserId = 13 };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Authorized user not found");
        }

    }
}
