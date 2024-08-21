using Social.Application.Features.Messages.DeleteMessage;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;

namespace Social.Application.Tests.Unit.Features.Messages
{
    public class DeleteMessageCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public DeleteMessageCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<DeleteMessageCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new DeleteMessageCommandHandler(_context)
                .Handle(c.Arg<DeleteMessageCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);        
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1, UserName = "tester" });
            context.Users.Add(new AppUser { Id = 2, UserName = "tester2" });

            context.PrivateMessages.Add(new PrivateMessage 
            { 
                PrivateMessageId = 1,
                Content = "test message",
                SenderId = 1,
                SenderUsername = "tester",
                RecipientId = 2,
                RecipientUsername = "tester2"
            });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task DeleteMessage_ShouldDeleteMessageForSenderAndReturnTrue_WhenMessageExists()
        {
            // Arrange
            var request = new DeleteMessageCommand
            {
                PrivateMessageId = 1,
                AuthUserId = 1
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<DeleteMessageResponse>();
            response.MessageDeleted.Should().BeTrue();
            _context.PrivateMessages.Should().NotBeEmpty();
            _context.PrivateMessages.Should().Contain(m => m.SenderDeleted == true);
        }

        [Fact]
        public async Task DeleteMessage_ShouldDeleteMessageForRecipientAndReturnTrue_WhenMessageExists()
        {
            // Arrange
            var request = new DeleteMessageCommand
            {
                PrivateMessageId = 1,
                AuthUserId = 2
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<DeleteMessageResponse>();
            response.MessageDeleted.Should().BeTrue();
            _context.PrivateMessages.Should().NotBeEmpty();
            _context.PrivateMessages.Should().Contain(m => m.RecipientDeleted == true);
        }

        [Fact]
        public async Task DeleteMessage_ShouldDeleteMessageFromDatabase_WhenMessageIsDeletedByBothUsers()
        {
            // Arrange
            _context.PrivateMessages.First().RecipientDeleted = true;

            var request = new DeleteMessageCommand
            {
                PrivateMessageId = 1,
                AuthUserId = 1
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<DeleteMessageResponse>();
            response.MessageDeleted.Should().BeTrue();
            _context.PrivateMessages.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteMessage_ShouldThrowNotFoundException_WhenMessageDoesNotExist()
        {
            // Arrange
            var request = new DeleteMessageCommand
            {
                PrivateMessageId = 2,
                AuthUserId = 1
            };

            // Act
            Func<Task> act = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Message not found");
        }

        [Fact]
        public async Task DeleteMessage_ShouldThrowUnauthorizedException_WhenUserIsNotSenderOrRecipient()
        {
            // Arrange
            var request = new DeleteMessageCommand
            {
                PrivateMessageId = 1,
                AuthUserId = 3
            };

            // Act
            Func<Task> act = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("You are not authorized to delete this message");
        }
    }
}
