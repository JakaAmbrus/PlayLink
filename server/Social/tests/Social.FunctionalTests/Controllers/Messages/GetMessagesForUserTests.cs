using Social.Application.Features.Messages.Common;
using Social.FunctionalTests.Configurations;
using Social.FunctionalTests.Models;

namespace Social.FunctionalTests.Controllers.Messages
{
    [Collection("Sequential")]
    public class GetMessagesForUserTests : BaseIntegrationTest
    {
        public GetMessagesForUserTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Users.Add(new AppUser { Id = 2, UserName = "testtwo" });

            var unreadMessages = Enumerable.Range(1, 5)
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
            Context.PrivateMessages.AddRange(unreadMessages);

            var inboxMessages = Enumerable.Range(6, 5)
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
            Context.PrivateMessages.AddRange(inboxMessages);

            var outboxMessages = Enumerable.Range(11, 5)
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
            Context.PrivateMessages.AddRange(outboxMessages);

            await Context.SaveChangesAsync();
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task GetMessagesForUser_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string url = $"/api/messages/user";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
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
            await InitializeTestAsync(new List<string> { "Member" });

            string url = $"/api/messages/user?pageNumber={pageNumber}&pageSize={pageSize}&container=Unread";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetUserMessagesResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.Messages.Count.Should().Be(expectedNumber);
            result.Messages.Should().AllBeOfType<MessageDto>();
            result.Messages[0].DateRead.Should().BeNull();
            result.Messages[0].SenderUsername.Should().Be("testtwo");
            result.Messages[0].RecipientUsername.Should().Be("authtester");
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
            await InitializeTestAsync(new List<string> { "Member" });

            string url = $"/api/messages/user?pageNumber={pageNumber}&pageSize={pageSize}&container=Inbox";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetUserMessagesResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.Messages.Count.Should().Be(expectedNumber);
            result.Messages.Should().AllBeOfType<MessageDto>();
            result.Messages[0].DateRead.Should().NotBeNull();
            result.Messages[0].SenderUsername.Should().Be("testtwo");
            result.Messages[0].RecipientUsername.Should().Be("authtester");
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
            await InitializeTestAsync(new List<string> { "Member" });

            string url = $"/api/messages/user?pageNumber={pageNumber}&pageSize={pageSize}&container=Outbox";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetUserMessagesResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.Messages.Count.Should().Be(expectedNumber);
            result.Messages.Should().AllBeOfType<MessageDto>();
            result.Messages[0].DateRead.Should().NotBeNull();
            result.Messages[0].SenderUsername.Should().Be("authtester");
            result.Messages[0].RecipientUsername.Should().Be("testtwo");
        }

        [Theory]
        [InlineData("Unread")]
        [InlineData("Inbox")]
        [InlineData("Outbox")]
        public async Task GetMessagesForUser_ShouldReturnEmptyList_WhenNoMessagesExist(string container)
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            Context.RemoveRange(Context.PrivateMessages);
            await Context.SaveChangesAsync();

            string url = $"/api/messages/user?container={container}";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetUserMessagesResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
           
            result.Messages.Should().BeEmpty();
        }
        [Fact]
        public async Task GetMessagesForUser_ShouldReturnEmptyList_WhenPageNumberIsGreaterThanTotalPages()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string url = $"/api/messages/user?pageNumber=100&pageSize=20&container=Unread";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetUserMessagesResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.Messages.Should().BeEmpty();
        }

        [Fact]
        public async Task GetMessagesForUser_ShouldThrowForbiddenStatusCode_WhenAuthUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            string url = $"/api/messages/user";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetMessagesForUser_ShouldThrowNotFoundException_WhenAuthUserDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            Context.RemoveRange(Context.PrivateMessages);
            Context.RemoveRange(Context.Users);
            await Context.SaveChangesAsync();

            string url = $"/api/messages/user";

            // Act
            var response = await Client.GetAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task GetMessagesForUser_ShouldThrowBadRequestValidationException_WhenPageNumberIsZero()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string url = $"/api/messages/user?pageNumber=0&pageSize=5&container=Unread";

            // Act
            var response = await Client.GetAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Errors.Should().HaveCount(1);
            errorResponse.Errors["Params.PageNumber"].Should().HaveCount(1);
            errorResponse.Errors["Params.PageNumber"][0].Should().Be("Page number must be greater than 0.");
        }

        [Fact]
        public async Task GetMessagesForUser_ShouldThrowBadRequestValidationException_WhenPageNumberIsLessThanZero()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string url = $"/api/messages/user?pageNumber=-1&pageSize=5&container=Unread";

            // Act
            var response = await Client.GetAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Errors.Should().HaveCount(1);
            errorResponse.Errors["Params.PageNumber"].Should().HaveCount(1);
            errorResponse.Errors["Params.PageNumber"][0].Should().Be("Page number must be greater than 0.");
        }
    }
}
