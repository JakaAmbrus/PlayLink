using Application.Exceptions;
using Application.Features.Messages.Common;
using Application.Features.Messages.SendMessage;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using MediatR;

namespace Application.Tests.Unit.Features.Messages
{
    public class SendMessageCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public SendMessageCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<SendMessageCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new SendMessageCommandHandler(_context)
                .Handle(c.Arg<SendMessageCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1, UserName = "tester" });
            context.Users.Add(new AppUser { Id = 2, UserName = "tester2" });

            context.PrivateMessages.Add(new PrivateMessage
            {
                PrivateMessageId = 2,
                Content = "test message",
                SenderId = 1,
                SenderUsername = "tester",
                RecipientId = 2,
                RecipientUsername = "tester2"
            });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task SendMessage_ShouldReturnMessageDto_WhenMessageSent()
        {
            // Arrange
            var request = new SendMessageCommand
            {
                AuthUserId = 1,
                CreateMessageDto = new CreateMessageDto
                {
                    RecipientUsername = "tester2",
                    Content = "test message"
                }
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<SendMessageResponse>();
            response.Message.Should().BeOfType<MessageDto>();
            response.Message.Content.Should().Be("test message");
            response.Message.SenderUsername.Should().Be("tester");
            response.Message.RecipientUsername.Should().Be("tester2");
            response.Message.PrivateMessageSent.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(5000));
        }

        [Fact]
        public async Task SendMessage_ShouldThrowNotFoundException_WhenSenderDoesNotExist()
        {
            // Arrange
            var request = new SendMessageCommand
            {
                AuthUserId = 3,
                CreateMessageDto = new CreateMessageDto
                {
                    RecipientUsername = "tester2",
                    Content = "test message"
                }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Sender not found");
        }

        [Fact]
        public async Task SendMessage_ShouldThrowNotFoundException_WhenRecipientDoesNotExist()
        {
            // Arrange
            var request = new SendMessageCommand
            {
                AuthUserId = 1,
                CreateMessageDto = new CreateMessageDto
                {
                    RecipientUsername = "tester3",
                    Content = "test message"
                }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Recipient not found");
        }

        [Fact]
        public async Task SendMessage_ShouldThrowBadRequestException_WhenRecipientIsSameAsSender()
        {
            // Arrange
            var request = new SendMessageCommand
            {
                AuthUserId = 1,
                CreateMessageDto = new CreateMessageDto
                {
                    RecipientUsername = "tester",
                    Content = "test message"
                }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<BadRequestException>()
                .WithMessage("You cannot send messages to yourself");
        }
    }
}
