using Social.Application.Features.Messages.MarkMessageAsRead;
using Social.Application.Interfaces;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;
using Social.UnitTests.Configurations;

namespace Social.UnitTests.Features.Messages
{
    public class MarkMessageAsReadCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public MarkMessageAsReadCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<MarkMessageAsReadCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new MarkMessageAsReadCommandHandler(_context)
                .Handle(c.Arg<MarkMessageAsReadCommand>(), c.Arg<CancellationToken>()));

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
                RecipientUsername = "tester2",
                DateRead = null
            });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task MarkMessageAsRead_ShouldMarkMessageAsReadReturnTrue_WhenMessageExists()
        {
            // Arrange
            var request = new MarkMessageAsReadCommand
            {
                MessageId = 1
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.MessageMarked.Should().BeTrue();
            _context.PrivateMessages.Find(1).DateRead.Should().NotBeNull();
        }

        [Fact]
        public async Task MarkMessageAsRead_ShouldThrowNotFoundException_WhenMessageDoesNotExist()
        {
            // Arrange
            var request = new MarkMessageAsReadCommand
            {
                MessageId = 2
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Message not found");
        }
    }
}
