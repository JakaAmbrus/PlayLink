using Social.Application.Features.Admin.AdminUserDelete;

namespace Social.Api.Tests.Integration.Controllers.Admin
{
    [Collection("Sequential")]
    public class DeleteUserTests : BaseIntegrationTest
    {
        public DeleteUserTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Users.Add(new AppUser { Id = 2, UserName = "testertwo" });
            Context.Users.Add(new AppUser { Id = 3, UserName = "testerthree" });

            var group = new Group("SampleGroup");
            group.Connections.Add(new Connection { ConnectionId = "2", Username = "testertwo" });
            Context.Groups.Add(group);

            Context.Posts.Add(new Post { PostId = 1, AppUserId = 2 });

            Context.Likes.Add(new Like { LikeId = 1, PostId = 1, AppUserId = 2 });

            Context.FriendRequests.Add(new FriendRequest { FriendRequestId = 1, SenderId = 2, ReceiverId = 3 });

            Context.Friendships.Add(new Friendship { FriendshipId = 1, User1Id = 2, User2Id = 3 });

            Context.PrivateMessages.Add(new PrivateMessage { PrivateMessageId = 1, SenderId = 2, RecipientId = 3 });

            await Context.SaveChangesAsync(CancellationToken.None);
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task DeleteUser_ShouldDeleteUserAndReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            int userId = 2;
            string url = $"/api/admin/delete-user/{userId}";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteUser_ShouldDeleteUserAndRelatedEntities_WhenUserExists()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            int userId = 2;
            string url = $"/api/admin/delete-user/{userId}";

            // Act
            await Client.DeleteAsync(url);

            RefreshContext();

            // Assert
            Context.Users.Count().Should().Be(2);
            Context.Connections.Count().Should().Be(0);
            Context.Posts.Count().Should().Be(0);
            Context.Likes.Count().Should().Be(0);
            Context.FriendRequests.Count().Should().Be(0);
            Context.PrivateMessages.Count().Should().Be(0);
            Context.Friendships.Count().Should().Be(0);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnUserDeletedTrue_WhenUserIsDeleted()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            int userId = 2;
            string url = $"/api/admin/delete-user/{userId}";

            // Act
            var response = await Client.DeleteAsync(url);
            var result = await response.Content.ReadFromJsonAsync<AdminUserDeleteResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.UserDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUser_ShouldThrowForbiddenStatusCode_WhenUserIsNotAdmin()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator" });

            int userId = 2;
            string url = $"/api/admin/delete-user/{userId}";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteUser_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            int userId = 5;
            string url = $"/api/admin/delete-user/{userId}";

            // Act
            var response = await Client.DeleteAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task DeleteUser_ShouldThrowNotFoundException_WhenAuthUserDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });
            var authUser = await Context.FindAsync<AppUser>(1);
            Context.Remove(authUser);
            await Context.SaveChangesAsync();

            int userId = 2;
            string url = $"/api/admin/delete-user/{userId}";

            // Act
            var response = await Client.DeleteAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Authorized user not found");
        }

        [Fact]
        public async Task DeleteUser_ShouldThrowUnauthorizedException_WhenUserIsNotAdmin()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });
            var authUser = await Context.FindAsync<AppUser>(1);
            await UserManager.RemoveFromRoleAsync(authUser, "Admin");

            int userId = 1;
            string url = $"/api/admin/delete-user/{userId}";

            // Act
            var response = await Client.DeleteAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(401);
            errorResponse.Message.Should().Be("Unauthorized, only an Admin can delete a user");
        }

        [Fact]
        public async Task DeleteUser_ShouldThrowBadRequestValidationExceptionWhen_WhenTheRolesAreInvalid()
        {
            // Arrange
            await ResetDatabaseAsync();
            await RoleManager.CreateAsync(new AppRole { Name = "InvalidRole" });
            await InitializeAuthenticatedClient(new List<string> { "Member", "Moderator", "Admin", "InvalidRole" });
            await InitializeTestSeedDataAsync();

            int userId = 2;
            string url = $"/api/admin/delete-user/{userId}";

            // Act
            var response = await Client.DeleteAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Errors.Should().HaveCount(1);
            errorResponse.Errors["AuthUserRoles"].Should().HaveCount(1);
            errorResponse.Errors["AuthUserRoles"][0].Should().Be("Invalid user role.");
        }
    }
}
