using Social.Application.Features.Friends.Common;
using Social.Application.Features.Friends.RespondToFriendRequest;
using Social.Application.Interfaces;
using Social.Domain.Entities;
using Social.Domain.Enums;
using Social.Domain.Exceptions;
using MediatR;
using Social.UnitTests.Configurations;

namespace Social.UnitTests.Features.Friends
{
    public class RespondToFriendRequestCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public RespondToFriendRequestCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            _cacheInvalidationService = Substitute.For<ICacheInvalidationService>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<RespondToFriendRequestCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new RespondToFriendRequestCommandHandler(_context, _cacheInvalidationService)
                                              .Handle(c.Arg<RespondToFriendRequestCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            var users = Enumerable.Range(1, 5)
                .Select(i => new AppUser
                {
                    Id = i,
                    UserName = $"{i}"
                }).ToList();
            context.Users.AddRange(users);

            context.FriendRequests.Add(new FriendRequest
            {
                FriendRequestId = 1,
                SenderId = 2,
                ReceiverId = 1,
                Status = FriendRequestStatus.Pending
            });
            context.FriendRequests.Add(new FriendRequest
            {
                FriendRequestId = 2,
                SenderId = 2,
                ReceiverId = 1,
                Status = FriendRequestStatus.Declined
            });
            context.FriendRequests.Add(new FriendRequest
            {
                FriendRequestId = 3,
                SenderId = 3,
                ReceiverId = 1,
                Status = FriendRequestStatus.Accepted
            });
            // For testing the case where the user is not the recipient
            context.FriendRequests.Add(new FriendRequest
            {
                FriendRequestId = 4,
                SenderId = 1,
                ReceiverId = 4,
                Status = FriendRequestStatus.Accepted
            });
            // For testing the case where the user is already friends with the sender
            context.FriendRequests.Add(new FriendRequest
            {
                FriendRequestId = 5,
                SenderId = 5,
                ReceiverId = 1,
                Status = FriendRequestStatus.Pending
            }); 

            context.Friendships.Add(new Friendship { User1Id = 1, User2Id = 5 });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task RespondToFriendRequest_ShouldAcceptPendingFriendRequest_WhenFriendRequestExistsAndIsAccept()
        {
            // Arrange
            var request = new RespondToFriendRequestCommand 
            { 
                AuthUserId = 1,
                FriendRequestResponse = new FriendRequestResponseDto
                {
                    FriendRequestId = 1,
                    Accept = true
                }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.RequestAccepted.Should().BeTrue();
            _context.FriendRequests.Should().HaveCount(5);
            _context.FriendRequests.Should().Contain(x => x.FriendRequestId == 1 && x.Status == FriendRequestStatus.Accepted);
        }

        [Fact]
        public async Task RespondToFriendRequest_ShouldDeclinePendingFriendRequest_WhenFriendRequestExistsAndIsDecline()
        {
            // Arrange
            var request = new RespondToFriendRequestCommand
            {
                AuthUserId = 1,
                FriendRequestResponse = new FriendRequestResponseDto
                {
                    FriendRequestId = 1,
                    Accept = false
                }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.RequestAccepted.Should().BeFalse();
            _context.FriendRequests.Should().HaveCount(5);
            _context.FriendRequests.Should().Contain(x => x.FriendRequestId == 1 && x.Status == FriendRequestStatus.Declined);
        }

        [Fact]
        public async Task RespondToFriendRequest_ShouldInvalidateCaches_WhenFriendRequestIsAccepted()
        {
            // Arrange
            var request = new RespondToFriendRequestCommand
            {
                AuthUserId = 1,
                FriendRequestResponse = new FriendRequestResponseDto
                {
                    FriendRequestId = 1,
                    Accept = true
                }
            };

            // Act
            await _mediator.Send(request);

            // Assert
            _cacheInvalidationService.Received(1).InvalidateUserFriendsCache(1);
            _cacheInvalidationService.Received(1).InvalidateUserFriendsCache(2);
            _cacheInvalidationService.Received(1).InvalidateFriendRequestsCache(1);
            _cacheInvalidationService.Received(1).InvalidateFriendRequestsCache(2);
            _cacheInvalidationService.Received(1).InvalidateFriendshipStatusCache(2, 1);
        }

        [Fact]
        public async Task RespondToFriendRequest_ShouldInvalidateCaches_WhenFriendRequestIsDeclined()
        {
            // Arrange
            var request = new RespondToFriendRequestCommand
            {
                AuthUserId = 1,
                FriendRequestResponse = new FriendRequestResponseDto
                {
                    FriendRequestId = 1,
                    Accept = false
                }
            };

            // Act
            await _mediator.Send(request);

            // Assert
            _cacheInvalidationService.Received(1).InvalidateFriendRequestsCache(1);
            _cacheInvalidationService.Received(1).InvalidateFriendRequestsCache(2);
            _cacheInvalidationService.Received(1).InvalidateFriendshipStatusCache(2, 1);
        }

        [Fact]
        public async Task RespondToFriendRequest_ShouldThrowKeyNotFoundException_WhenFriendRequestDoesNotExist()
        {
            // Arrange
            var request = new RespondToFriendRequestCommand
            {
                AuthUserId = 1,
                FriendRequestResponse = new FriendRequestResponseDto
                {
                    FriendRequestId = 10,
                    Accept = true
                }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Friend request not found");
        }

        [Fact]
        public async Task RespondToFriendRequest_ShouldThrowUnauthorizedAccessException_WhenUserIsNotTheRecipient()
        {
            // Arrange
            var request = new RespondToFriendRequestCommand
            {
                AuthUserId = 1,
                FriendRequestResponse = new FriendRequestResponseDto
                {
                    FriendRequestId = 4,
                    Accept = true
                }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("You are not authorized to respond to this friend request");
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public async Task RespondToFriendRequest_ShouldThrowBadRequestException_WhenFriendRequestIsNotPending(int id)
        {
            // Arrange
            var request = new RespondToFriendRequestCommand
            {
                AuthUserId = 1,
                FriendRequestResponse = new FriendRequestResponseDto
                {
                    FriendRequestId = id,
                    Accept = true
                }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<BadRequestException>()
                .WithMessage("This friend request already has a response");
        }

        [Fact]
        public async Task RespondToFriendRequest_ShouldThrowBadRequestException_WhenUsersAreAlreadyFriends()
        {
            // Arrange
            var request = new RespondToFriendRequestCommand
            {
                AuthUserId = 1,
                FriendRequestResponse = new FriendRequestResponseDto
                {
                    FriendRequestId = 5,
                    Accept = true
                }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<BadRequestException>()
                .WithMessage("You are already friends with this user");
        }
    }
}
