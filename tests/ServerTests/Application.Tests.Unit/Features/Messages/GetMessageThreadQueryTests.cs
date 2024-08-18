using Application.Features.Messages.Common;
using Application.Features.Messages.GetMessageThread;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Tests.Unit.Features.Messages
{
    public class GetMessageThreadQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public GetMessageThreadQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetMessageThreadQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetMessageThreadQueryHandler(_context)
                .Handle(c.Arg<GetMessageThreadQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1, UserName = "tester" });
            context.Users.Add(new AppUser { Id = 2, UserName = "tester2" });
            context.Users.Add(new AppUser { Id = 3, UserName = "tester3" });

            var sentUnreadMessages = Enumerable.Range(1, 3)
                .Select(i => new PrivateMessage
                {
                PrivateMessageId = i,
                Content = "test message",
                SenderId = 1,
                SenderUsername = "tester",
                RecipientId = 2,
                RecipientUsername = "tester2",
                DateRead = null,
            }).ToList();
            context.PrivateMessages.AddRange(sentUnreadMessages);

            var sentMessages = Enumerable.Range(4, 3)
                .Select(i => new PrivateMessage
                {
                PrivateMessageId = i,
                Content = "test message",
                SenderId = 1,
                SenderUsername = "tester",
                RecipientId = 2,
                RecipientUsername = "tester2",
                DateRead = DateTime.UtcNow,
            }).ToList();
            context.PrivateMessages.AddRange(sentMessages);

            var receivedUnreadMessages = Enumerable.Range(7, 3)
                .Select(i => new PrivateMessage
                {
                PrivateMessageId = i,
                Content = "test message",
                SenderId = 2,
                SenderUsername = "tester2",
                RecipientId = 1,
                RecipientUsername = "tester",
                DateRead = null,
            }).ToList();
            context.PrivateMessages.AddRange(receivedUnreadMessages);

            var receivedMessages = Enumerable.Range(10, 3)
                .Select(i => new PrivateMessage
                {
                PrivateMessageId = i,
                Content = "test message",
                SenderId = 2,
                SenderUsername = "tester2",
                RecipientId = 1,
                RecipientUsername = "tester",
                DateRead = DateTime.UtcNow,
            }).ToList();
            context.PrivateMessages.AddRange(receivedMessages);

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetMessageThread_ShouldReturnMessageThreadDto_WhenMessagesExist()
        {
            // Arrange
            var request = new GetMessageThreadQuery
            {
                ProfileUsername = "tester2",
                AuthUserId = 1
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<GetMessageThreadResponse>();
            response.Messages.Should().NotBeEmpty();
            response.Messages.Should().AllBeOfType<MessageDto>();
            response.Messages.Count.Should().Be(12);
        }

        [Fact]
        public async Task GetMessageThread_ShouldReturnEmptyMessageThreadDto_WhenUserHasNoMessages()
        {
            // Arrange
            var request = new GetMessageThreadQuery
            {
                ProfileUsername = "tester3",
                AuthUserId = 1
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<GetMessageThreadResponse>();
            response.Messages.Should().BeEmpty();
        }

        [Fact]
        public async Task GetMessageThread_ShouldMarkRecipientMessagesAsRead_WhenMessagesExist()
        {
            // Arrange
            var request = new GetMessageThreadQuery
            {
                ProfileUsername = "tester2",
                AuthUserId = 1
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            _context.PrivateMessages.Should().Contain(m => m.RecipientId == 1 && m.DateRead != null);
        }

        [Fact]
        public async Task GetMessageThread_ShouldThrowNotFoundException_WhenProfileUserDoesNotExist()
        {
            // Arrange
            var request = new GetMessageThreadQuery
            {
                ProfileUsername = "tester4",
                AuthUserId = 1
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Profile user not found");
        }

        [Fact]
        public async Task GetMessageThread_ShouldThrowNotFoundException_WhenAuthorizedUserDoesNotExist()
        {
            // Arrange
            var request = new GetMessageThreadQuery
            {
                ProfileUsername = "tester2",
                AuthUserId = 4
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Authorized user not found");
        }
    }
}
