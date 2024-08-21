using Social.Application.Features.Friends.SendFriendRequest;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Domain.Entities;
using Social.Domain.Enums;
using Social.Domain.Exceptions;
using MediatR;

namespace Social.Application.Tests.Unit.Features.Friends
{
    public class SendFriendRequestCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public SendFriendRequestCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            _cacheInvalidationService = Substitute.For<ICacheInvalidationService>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<SendFriendRequestCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new SendFriendRequestCommandHandler(_context, _cacheInvalidationService)
                .Handle(c.Arg<SendFriendRequestCommand>(), c.Arg<CancellationToken>()));

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
                SenderId = 1,
                ReceiverId = 3,
                Status = FriendRequestStatus.Declined
            });

            context.FriendRequests.Add(new FriendRequest
            {
                FriendRequestId = 2,
                SenderId = 1,
                ReceiverId = 4,
                Status = FriendRequestStatus.Pending
            });

            context.Friendships.Add(new Friendship { User1Id = 1, User2Id = 5 });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task SendFriendRequest_ShouldSendFriendRequest_WhenFriendRequestDoesNotExist()
        {
            // Arrange
            var request = new SendFriendRequestCommand { AuthUserId = 1, ReceiverUsername = "2" };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.RequestSent.Should().BeTrue();
        }

        [Fact]
        public async Task SendFriendRequest_ShouldInvalidateCache_WhenFriendRequestIsSent()
        {
            // Arrange
            var request = new SendFriendRequestCommand { AuthUserId = 1, ReceiverUsername = "2" };

            // Act
            await _mediator.Send(request);

            // Assert
            _cacheInvalidationService.Received(1).InvalidateFriendRequestsCache(2);
            _cacheInvalidationService.Received(1).InvalidateFriendshipStatusCache(1, 2);
        }   

        [Fact]
        public async Task SendFriendRequest_ShouldThrowNotFoundException_WhenReceiverDoesNotExist()
        {
            // Arrange
            var request = new SendFriendRequestCommand { AuthUserId = 1, ReceiverUsername = "6" };

            // Act
            Func<Task> act = async () => { await _mediator.Send(request); };

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Receiver not found");
        }

        [Fact]
        public async Task SendFriendRequest_ShouldThrowBadRequestException_WhenUsersAreAlreadyFriends()
        {
            // Arrange
            var request = new SendFriendRequestCommand { AuthUserId = 1, ReceiverUsername = "5" };

            // Act
            Func<Task> action = async () => { await _mediator.Send(request); };

            // Assert
            await action.Should().ThrowAsync<BadRequestException>()
                .WithMessage("You are already friends with this user");
        }

        [Fact]
        public async Task SendFriendRequest_ShouldThrowBadRequestException_WhenTryingToSendYourselfAFriendRequest()
        {
            // Arrange
            var request = new SendFriendRequestCommand { AuthUserId = 1, ReceiverUsername = "1" };

            // Act
            Func<Task> action = async () => { await _mediator.Send(request); };

            // Assert
            await action.Should().ThrowAsync<BadRequestException>()
                .WithMessage("You cannot send a friend request to yourself");
        }

        [Fact]
        public async Task SendFriendRequest_ShouldThrowBadRequestException_WhenFriendRequestIsDeclined()
        {
            // Arrange
            var request = new SendFriendRequestCommand { AuthUserId = 1, ReceiverUsername = "3" };

            // Act
            Func<Task> action = async () => { await _mediator.Send(request); };

            // Assert
            await action.Should().ThrowAsync<BadRequestException>()
                .WithMessage("A previous friend request was declined, wait for the owner to get notified before resending");
        }

        [Fact]
        public async Task SendFriendRequest_ShouldThrowBadRequestException_WhenFriendRequestIsAlreadyPending()
        {
            // Arrange
            var request = new SendFriendRequestCommand { AuthUserId = 1, ReceiverUsername = "4" };

            // Act
            Func<Task> action = async () => { await _mediator.Send(request); };

            // Assert
            await action.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Friend request already sent");
        }
    }
}
