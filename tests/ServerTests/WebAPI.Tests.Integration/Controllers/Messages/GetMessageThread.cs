using Application.Features.Messages.Common;
using Application.Features.Messages.GetMessageThread;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Tests.Integration.Controllers.Messages
{
    [Collection("Sequential")]
    public class GetMessageThread : BaseIntegrationTest
    {
        public GetMessageThread(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Users.Add(new AppUser { Id = 2, UserName = "testtwo" });
            Context.Users.Add(new AppUser { Id = 3, UserName = "testthree" });

            var sentUnreadMessages = Enumerable.Range(1, 3)
                .Select(i => new PrivateMessage
                {
                    PrivateMessageId = i,
                    Content = "test message",
                    SenderId = 1,
                    SenderUsername = "authtester",
                    RecipientId = 2,
                    RecipientUsername = "testtwo",
                    DateRead = null,
                }).ToList();
            Context.PrivateMessages.AddRange(sentUnreadMessages);

            var sentMessages = Enumerable.Range(4, 3)
                .Select(i => new PrivateMessage
                {
                    PrivateMessageId = i,
                    Content = "test message",
                    SenderId = 1,
                    SenderUsername = "authtester",
                    RecipientId = 2,
                    RecipientUsername = "testtwo",
                    DateRead = DateTime.UtcNow,
                }).ToList();
            Context.PrivateMessages.AddRange(sentMessages);

            var receivedUnreadMessages = Enumerable.Range(7, 3)
                .Select(i => new PrivateMessage
                {
                    PrivateMessageId = i,
                    Content = "test message",
                    SenderId = 2,
                    SenderUsername = "testtwo",
                    RecipientId = 1,
                    RecipientUsername = "authtester",
                    DateRead = null,
                }).ToList();
            Context.PrivateMessages.AddRange(receivedUnreadMessages);

            var receivedMessages = Enumerable.Range(10, 3)
                .Select(i => new PrivateMessage
                {
                    PrivateMessageId = i,
                    Content = "test message",
                    SenderId = 2,
                    SenderUsername = "testtwo",
                    RecipientId = 1,
                    RecipientUsername = "authtester",
                    DateRead = DateTime.UtcNow,
                }).ToList();
            Context.PrivateMessages.AddRange(receivedMessages);

            await Context.SaveChangesAsync();
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task GetMessageThread_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string recipientUsername = "testtwo";
            string url = $"/api/messages/thread/{recipientUsername}";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetMessageThread_ShouldReturnMessageThreadDto_WhenMessagesExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string recipientUsername = "testtwo";
            string url = $"/api/messages/thread/{recipientUsername}";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetMessageThreadResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Messages.Should().AllBeOfType<MessageDto>();
            result.Messages.Should().HaveCount(12);          
        }

        [Fact]
        public async Task GetMessageThread_ShouldReturnEmptyMessageThreadDto_WhenUserHasNoMessages()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string recipientUsername = "testthree";
            string url = $"/api/messages/thread/{recipientUsername}";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetMessageThreadResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Messages.Should().BeEmpty();
        }

        [Fact]
        public async Task GetMessageThread_ShouldMarkRecipientMessagesAsRead_WhenMessagesExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string recipientUsername = "testtwo";
            string url = $"/api/messages/thread/{recipientUsername}";

            // Act
            await Client.GetAsync(url);

            RefreshContext();

            // Assert
            Context.PrivateMessages.Should().Contain(m => m.RecipientId == 1 && m.DateRead != null);
        }

        [Fact]
        public async Task GetMessageThread_ShouldThrowForbiddenStatusCode_WhenAuthUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            string recipientUsername = "testtwo";
            string url = $"/api/messages/thread/{recipientUsername}";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetMessageThread_ShouldThrowNotFoundException_WhenProfileUserDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string recipientUsername = "testfour";
            string url = $"/api/messages/thread/{recipientUsername}";

            // Act
            var response = await Client.GetAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Profile user not found");
        }

        [Fact]
        public async Task GetMessageThread_ShouldThrowNotFoundException_WhenAuthorizedUserDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            Context.RemoveRange(Context.PrivateMessages);
            var authUsr = await Context.Users.FindAsync(1);
            Context.Remove(authUsr);
            await Context.SaveChangesAsync();

            string recipientUsername = "testtwo";
            string url = $"/api/messages/thread/{recipientUsername}";

            // Act
            var response = await Client.GetAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Authorized user not found");
        }
    }
}
