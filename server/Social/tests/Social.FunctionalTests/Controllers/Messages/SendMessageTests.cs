using Social.Application.Features.Messages.Common;
using Social.Application.Features.Messages.SendMessage;
using Social.FunctionalTests.Configurations;

namespace Social.FunctionalTests.Controllers.Messages
{
    [Collection("Sequential")]
    public class SendMessageTests : BaseIntegrationTest
    {
        public SendMessageTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Users.Add(new AppUser { Id = 2, UserName = "testtwo" });

            Context.PrivateMessages.Add(new PrivateMessage
            {
                PrivateMessageId = 2,
                Content = "test message",
                SenderId = 1,
                SenderUsername = "authtester",
                RecipientId = 2,
                RecipientUsername = "testtwo"
            });

            await Context.SaveChangesAsync();
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task SendMessage_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string url = "/api/messages";
            var message = new CreateMessageDto
            {
                    RecipientUsername = "testtwo",
                    Content = "test message"
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, message);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task SendMessage_ShouldReturnMessageDto_WhenMessageSent()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string url = "/api/messages";
            var message = new CreateMessageDto
            {
                RecipientUsername = "testtwo",
                Content = "test message"
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, message);
            var result = await response.Content.ReadFromJsonAsync<SendMessageResponse>();

            // Assert
            result.Message.Should().BeOfType<MessageDto>();
            result.Message.Content.Should().Be("test message");
            result.Message.SenderUsername.Should().Be("authtester");
            result.Message.RecipientUsername.Should().Be("testtwo");
        }

        [Fact]
        public async Task SendMessage_ShouldThrowForbiddenStatusCode_WhenUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            string url = "/api/messages";
            var message = new CreateMessageDto
            {
                RecipientUsername = "testtwo",
                Content = "test message"
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, message);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task SendMessage_ShouldThrowNotFoundException_WhenSenderDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            Context.RemoveRange(Context.PrivateMessages);
            var authUser = await Context.FindAsync<AppUser>(1);
            Context.Remove(authUser);
            await Context.SaveChangesAsync();

            string url = "/api/messages";
            var message = new CreateMessageDto
            {
                RecipientUsername = "testtwo",
                Content = "test message"
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, message);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Sender not found");
        }

        [Fact]
        public async Task SendMessage_ShouldThrowNotFoundException_WhenRecipientDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string url = "/api/messages";
            var message = new CreateMessageDto
            {
                RecipientUsername = "testthree",
                Content = "test message"
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, message);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Contain("Recipient not found");
        }

        [Fact]
        public async Task SendMessage_ShouldThrowBadRequestException_WhenRecipientIsSameAsSender()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string url = "/api/messages";
            var message = new CreateMessageDto
            {
                RecipientUsername = "authtester",
                Content = "test message"
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, message);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Message.Should().Contain("You cannot send messages to yourself");
        }

        [Fact]
        public async Task SendMessage_ShouldThrowBadRequestValidationException_WhenTheContentExceedsMaxLength()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string url = "/api/messages";
            var message = new CreateMessageDto
            {
                RecipientUsername = "testtwo",
                Content = new string('a', 501)
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, message);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Errors["CreateMessageDto.Content"].Should().Contain("Message cannot exceed 500 characters");
        }
    }
}
