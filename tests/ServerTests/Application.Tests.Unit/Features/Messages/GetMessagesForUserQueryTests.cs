using Application.Features.Messages.Common;
using Application.Features.Messages.GetMessagesForUser;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Application.Utils;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Tests.Unit.Features.Messages
{
    public class GetMessagesForUserQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public GetMessagesForUserQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetMessagesForUserQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetMessagesForUserQueryHandler(_context)
                .Handle(c.Arg<GetMessagesForUserQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1, UserName = "tester" });
            context.Users.Add(new AppUser { Id = 2, UserName = "tester2" });

            var unreadMessages = Enumerable.Range(1, 5)
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
            context.PrivateMessages.AddRange(unreadMessages);

            var inboxMessages = Enumerable.Range(6, 5)
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
            context.PrivateMessages.AddRange(inboxMessages);

            var outboxMessages = Enumerable.Range(11, 5)
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
            context.PrivateMessages.AddRange(outboxMessages);

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Theory]
        [InlineData(1, 5, 5)]
        [InlineData(1, 10, 5)]
        [InlineData(1, 2, 2)]
        [InlineData(2, 2, 2)]
        public async Task GetMessagesForUser_ShouldReturnPagedListOfUnreadMessageDTOs_WhenUnreadMessagesExist(
            int pageNumber, int pageSize, int expectedNumber)
        {
            // Arrange
            var request = new GetMessagesForUserQuery
            {
                Params = new MessageParams 
                { 
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Container = "Unread"
                },
                AuthUserId = 1
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Messages.Should().NotBeNull();
            response.Messages.Count().Should().Be(expectedNumber);
            response.Messages.Should().AllBeOfType<MessageDto>();
            response.Messages[0].DateRead.Should().BeNull();
            response.Messages[0].SenderUsername.Should().Be("tester2");
        }

        [Theory]
        [InlineData(1, 5, 5)]
        [InlineData(1, 10, 10)]
        [InlineData(1, 2, 2)]
        [InlineData(2, 2, 2)]
        public async Task GetMessagesForUser_ShouldReturnPagedListOfInboxMessageDTOs_WhenInboxMessagesExist(
            int pageNumber, int pageSize, int expectedNumber)
        {
            // Arrange
            var request = new GetMessagesForUserQuery
            {
                Params = new MessageParams
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Container = "Inbox"
                },
                AuthUserId = 1
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Messages.Should().NotBeNull();
            response.Messages.Count().Should().Be(expectedNumber);
            response.Messages.Should().AllBeOfType<MessageDto>();
            response.Messages[0].DateRead.Should().NotBeNull();
            response.Messages[0].SenderUsername.Should().Be("tester2");
        }

        [Theory]
        [InlineData(1, 5, 5)]
        [InlineData(1, 10, 5)]
        [InlineData(1, 2, 2)]
        [InlineData(2, 2, 2)]
        public async Task GetMessagesForUser_ShouldReturnPagedListOfOutboxMessageDTOs_WhenOutboxMessagesExist(
            int pageNumber, int pageSize, int expectedNumber)
        {
            // Arrange
            var request = new GetMessagesForUserQuery
            {
                Params = new MessageParams
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Container = "Outbox"
                },
                AuthUserId = 1
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Messages.Should().NotBeNull();
            response.Messages.Count().Should().Be(expectedNumber);
            response.Messages.Should().AllBeOfType<MessageDto>();
            response.Messages[0].DateRead.Should().NotBeNull();
            response.Messages[0].RecipientUsername.Should().Be("tester2");
        }

        [Theory]
        [InlineData("Unread")]
        [InlineData("Inbox")]
        [InlineData("Outbox")]
        public async Task GetMessagesForUser_ShouldReturnEmptyList_WhenNoMessagesExist(string container)
        {
            // Arrange
            _context.PrivateMessages.RemoveRange(_context.PrivateMessages);
            _context.SaveChangesAsync(CancellationToken.None).Wait();

            var request = new GetMessagesForUserQuery
            {
                Params = new MessageParams
                {
                    PageNumber = 1,
                    PageSize = 5,
                    Container = container
                },
                AuthUserId = 2
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Messages.Should().NotBeNull();
            response.Messages.Count().Should().Be(0);
        }

        [Theory]
        [InlineData("Unread")]
        [InlineData("Inbox")]
        [InlineData("Outbox")]
        public async Task GetMessagesForUser_ShouldReturnEmptyList_whenPageNumberIsGreaterThanTotalPages(string container)
        {
            // Arrange
            var request = new GetMessagesForUserQuery
            {
                Params = new MessageParams
                {
                    PageNumber = 5,
                    PageSize = 5,
                    Container = container
                },
                AuthUserId = 1
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Messages.Should().NotBeNull();
            response.Messages.Count().Should().Be(0);
        }

        [Fact]
        public async Task GetMessagesForUser_ShouldThrowNotFoundException_WhenAuthUserDoesNotExist()
        {
            // Arrange
            var request = new GetMessagesForUserQuery
            {
                Params = new MessageParams
                {
                    PageNumber = 1,
                    PageSize = 5,
                    Container = "Unread"
                },
                AuthUserId = 3
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User not found");
        }
    }
}
