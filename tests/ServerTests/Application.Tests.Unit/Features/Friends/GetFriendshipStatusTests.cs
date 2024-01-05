using Application.Exceptions;
using Application.Features.Friends.GetRelationshipStatus;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Tests.Unit.Features.Friends
{
    public class GetFriendshipStatusTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly ICacheKeyService _cacheKeyService;

        public GetFriendshipStatusTests()
        {
            _context = TestBase.CreateTestDbContext();
            _memoryCache = Substitute.For<IMemoryCache>();
            _cacheKeyService = Substitute.For<ICacheKeyService>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetFriendshipStatusQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetFriendshipStatusQueryHandler(_context, _memoryCache, _cacheKeyService)
                .Handle(c.Arg<GetFriendshipStatusQuery>(), c.Arg<CancellationToken>()));

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

            context.Friendships.Add(new Friendship { User1Id = 1, User2Id = 2 });

            context.FriendRequests.Add(new FriendRequest 
            { 
                SenderId = 1,
                ReceiverId = 3,
                Status = FriendRequestStatus.Pending
            });
            context.FriendRequests.Add(new FriendRequest
            { 
                SenderId = 2,
                ReceiverId = 3,
                Status = FriendRequestStatus.Declined
            });
           
            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Theory]
        [InlineData(1, "2", FriendshipStatus.Friends)]   
        [InlineData(1, "3", FriendshipStatus.Pending)]  
        [InlineData(2, "3", FriendshipStatus.Declined)] 
        [InlineData(3, "4", FriendshipStatus.None)]    
        public async Task GetFriendshipStatus_ShouldReturnCorrectFriendshipStatus_WhenThereIsNoCache(
            int Id, string username, FriendshipStatus expectedStatus)
        {
            // Arrange
            var request = new GetFriendshipStatusQuery { AuthUserId = Id, ProfileUsername = username };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Status.Should().Be(expectedStatus);
        }

        [Theory]
        [InlineData(1, "2", FriendshipStatus.Friends)] 
        [InlineData(1, "3", FriendshipStatus.Pending)]
        [InlineData(2, "3", FriendshipStatus.Declined)]
        [InlineData(3, "4", FriendshipStatus.None)]
        public async Task Handle_ShouldReturnCorrectFriendshipStatus_FromCache(int authUserId, string profileUsername, FriendshipStatus expectedStatus)
        {
            // Arrange
            var request = new GetFriendshipStatusQuery { AuthUserId = authUserId, ProfileUsername = profileUsername };
            var cacheKey = $"FriendshipStatus-{authUserId}-{profileUsername}"; 
            var cachedResponse = new GetFriendshipStatusResponse { Status = expectedStatus };

            _memoryCache.TryGetValue(cacheKey, out Arg.Any<GetFriendshipStatusResponse>())
              .Returns(x =>
              {
                  x[1] = cachedResponse;
                  return true;
              });

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Status.Should().Be(expectedStatus);
        }

        [Fact]
        public async Task GetFriendshipStatus_ShouldThrowNotFoundException_WhenProfileUserNotFound()
        {
            // Arrange
            var request = new GetFriendshipStatusQuery {  ProfileUsername = "ImaginaryUser", AuthUserId = 1 };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Profile user not found");
        }
    }
}
