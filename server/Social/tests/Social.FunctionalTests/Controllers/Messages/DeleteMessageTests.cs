using Social.Application.Features.Messages.DeleteMessage;
using Social.FunctionalTests.Configurations;

namespace Social.FunctionalTests.Controllers.Messages
{
    [Collection("Sequential")]
    public class DeleteMessageTests : BaseIntegrationTest
    {
        public DeleteMessageTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Users.Add(new AppUser { Id = 2, UserName = "testtwo" });

            Context.PrivateMessages.Add(new PrivateMessage
            {
                PrivateMessageId = 1,
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
        public async Task DeleteMessage_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string url = "/api/messages/1";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteMessage_ShouldDeleteMessageForSenderAndReturnTrue_WhenMessageExists()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string url = "/api/messages/1";

            // Act
            var response = await Client.DeleteAsync(url);
            var result = await response.Content.ReadFromJsonAsync<DeleteMessageResponse>();

            RefreshContext();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.Should().BeOfType<DeleteMessageResponse>();
            result.MessageDeleted.Should().BeTrue();
            Context.PrivateMessages.Should().NotBeEmpty();
            Context.PrivateMessages.Should().Contain(m => m.SenderDeleted == true);
        }

        [Fact]
        public async Task DeleteMessage_ShouldDeleteMessageForRecipientAndReturnTrue_WhenMessageExists()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            Context.PrivateMessages.Add(new PrivateMessage
            {
                PrivateMessageId = 2,
                Content = "test message",
                SenderId = 2,
                SenderUsername = "testtwo",
                RecipientId = 1,
                RecipientUsername = "authtester"
            });
            await Context.SaveChangesAsync();

            string url = "/api/messages/2";

            // Act
            var response = await Client.DeleteAsync(url);
            var result = await response.Content.ReadFromJsonAsync<DeleteMessageResponse>();

            RefreshContext();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.Should().BeOfType<DeleteMessageResponse>();
            result.MessageDeleted.Should().BeTrue();
            Context.PrivateMessages.Should().NotBeEmpty();
            Context.PrivateMessages.Should().Contain(m => m.RecipientDeleted == true);
        }

        [Fact]
        public async Task DeleteMessage_ShouldDeleteMessageFromDatabase_WhenMessageIsDeletedByBothUsers()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            Context.PrivateMessages.First().RecipientDeleted = true;
            await Context.SaveChangesAsync();

            string url = "/api/messages/1";

            // Act
            var response = await Client.DeleteAsync(url);
            var result = await response.Content.ReadFromJsonAsync<DeleteMessageResponse>();

            RefreshContext();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.Should().BeOfType<DeleteMessageResponse>();
            result.MessageDeleted.Should().BeTrue();
            Context.PrivateMessages.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteMessage_ShouldThrowForbiddenStatusCode_WhenUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            string url = "/api/messages/1";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteMessage_ShouldThrowNotFoundException_WhenMessageDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string url = "/api/messages/2";

            // Act
            var response = await Client.DeleteAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Message not found");
        }

        [Fact]
        public async Task DeleteMessage_ShouldThrowUnauthorizedException_WhenUserIsNotSenderOrRecipient()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            Context.Users.Add(new AppUser { Id = 3, UserName = "testthree" });
            Context.Users.Add(new AppUser { Id = 4, UserName = "testfour" });
            Context.Add(new PrivateMessage
            {
                PrivateMessageId = 2,
                Content = "test message",
                SenderId = 3,
                SenderUsername = "testthree",
                RecipientId = 4,
                RecipientUsername = "testfour"
            });
            await Context.SaveChangesAsync();

            string url = "/api/messages/2";

            // Act
            var response = await Client.DeleteAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(401);
            errorResponse.Message.Should().Be("You are not authorized to delete this message");
        }
    }
}
