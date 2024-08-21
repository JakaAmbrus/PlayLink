using Social.Application.Features.Posts.DeletePost;

namespace Social.Api.Tests.Integration.Controllers.Posts
{
    public class DeletePostTests : BaseIntegrationTest
    {
        public DeletePostTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Users.Add(new AppUser { Id = 2, UserName = "tester" });

            Context.Posts.Add(new Post { PostId = 1, AppUserId = 1, CommentsCount = 1 });
            Context.Posts.Add(new Post { PostId = 2, AppUserId = 2, CommentsCount = 0 });

            Context.Comments.Add(new Comment
            {
                CommentId = 1,
                PostId = 1,
                AppUserId = 1,
                Content = "Comment 1",
                TimeCommented = DateTime.UtcNow.AddDays(-1)
            });

            Context.Likes.Add(new Like { PostId = 1, AppUserId = 1 });
            Context.Likes.Add(new Like { PostId = 1, AppUserId = 2 });

            await Context.SaveChangesAsync();
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task DeletePost_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeletePost_ShouldDeletePostAndReturnTheCorrectResponse_WhenUserIsTheOwner()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.DeleteAsync(url);
            var result = await response.Content.ReadFromJsonAsync<DeletePostResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeletePost_ShouldDeletePostAndReturnTheCorrectResponse_WhenUserIsModerator()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator" });

            int postId = 2;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.DeleteAsync(url);
            var result = await response.Content.ReadFromJsonAsync<DeletePostResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeletePost_ShouldRemovePostFromDB_WhenPostIsDeleted()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.DeleteAsync(url);

            RefreshContext();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Posts.Count().Should().Be(1);
        }

        [Fact]
        public async Task DeletePost_ShouldDeleteCommentsAndLikes_WhenPostIsDeleted()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.DeleteAsync(url);

            RefreshContext();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Comments.Count().Should().Be(0);
            Context.Likes.Count().Should().Be(0);
        }

        [Fact]
        public async Task DeletePost_ShouldThrowForbiddenStatusCode_WhenUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            int postId = 1;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeletePost_ShouldThrowUnauthorizedException_WhenUserIsNotTheOwnerAndNotModerator()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 2;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.DeleteAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(401);
            errorResponse.Message.Should().Be("User not authorized to delete this post");
        }

        [Fact]
        public async Task DeletePost_ShouldThrowNotFoundException_WhenAuthorizedUserDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}";

            var authUser = await Context.Users.FindAsync(1);
            Context.Remove(authUser);
            await Context.SaveChangesAsync();

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
        public async Task DeletePost_ShouldThrowNotFoundException_WhenPostDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 3;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.DeleteAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Post was not found");
        }

        [Fact]
        public async Task DeletePost_ShouldThrowBadRequestValidationExceptionWhen_WhenTheRolesAreInvalid()
        {
            // Arrange
            await ResetDatabaseAsync();
            await RoleManager.CreateAsync(new AppRole { Name = "InvalidRole" });
            await InitializeAuthenticatedClient(new List<string> { "Member", "InvalidRole" });
            await InitializeTestSeedDataAsync();

            int postId = 1;
            string url = $"/api/posts/{postId}";

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
