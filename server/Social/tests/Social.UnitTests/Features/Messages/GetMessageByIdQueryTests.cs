using Social.Application.Features.Messages.Common;
using Social.Application.Features.Messages.GetMessageById;
using Social.Application.Interfaces;
using Social.Domain.Entities;
using MediatR;
using Social.UnitTests.Configurations;

namespace Social.UnitTests.Features.Messages
{
    public class GetMessageByIdQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public GetMessageByIdQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetMessageByIdQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetMessageByIdQueryHandler(_context)
                .Handle(c.Arg<GetMessageByIdQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser 
            { 
                Id = 1,
                UserName = "tester",
                FullName = "Test Tester",
                Gender = "male",
                ProfilePictureUrl = "https://Coudinary.com",
            });
            context.Users.Add(new AppUser { Id = 2, UserName = "tester2" });

            context.PrivateMessages.Add(new PrivateMessage
            {
                PrivateMessageId = 1,
                Content = "test message",
                SenderId = 1,
                SenderUsername = "tester",
                RecipientId = 2,
                RecipientUsername = "tester2",
                DateRead = null,
                PrivateMessageSent = DateTime.UtcNow
            });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetMessageById_ShouldReturnMessageDto_WhenMessageExists()
        {
            // Arrange
            var request = new GetMessageByIdQuery
            {
                MessageId = 1
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<MessageDto>();
            response.Content.Should().Be("test message");
            response.SenderUsername.Should().Be("tester");
            response.RecipientUsername.Should().Be("tester2");

        }
    }
}
