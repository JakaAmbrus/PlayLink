using Social.Application.Features.Likes.UnlikePost;
using Social.FunctionalTests.Configurations;

namespace Social.FunctionalTests.Controllers.Posts
{
    [Collection("Sequential")]
    public class UnlikePostTests : BaseIntegrationTest
    {
        public UnlikePostTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Posts.Add(new Post { PostId = 1, AppUserId = 1, LikesCount = 1 });
            Context.Posts.Add(new Post { PostId = 2, AppUserId = 1, LikesCount = 0 });

            Context.Likes.Add(new Like { PostId = 1, AppUserId = 1 });

            await Context.SaveChangesAsync();
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task UnlikePost_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}/like";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UnlikePost_ShouldUnlikePostAndDecrementLikesCount_WhenLikedPostExists()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}/like";

            // Act
            var response = await Client.DeleteAsync(url);

            RefreshContext();

            // Assert
            var post = Context.Posts.Find(postId);
            post.LikesCount.Should().Be(0);
        }

        [Fact]
        public async Task UnlikePost_ShouldReturnUnlikedTrue_WhenLikedPostExists()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}/like";

            // Act
            var response = await Client.DeleteAsync(url);
            var result = await response.Content.ReadFromJsonAsync<UnlikePostResponse>();

            // Assert      
            result.Unliked.Should().BeTrue();
        }

        [Fact]
        public async Task UnlikePost_ShouldThrowForbiddenStatusCode_WhenUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            int postId = 1;
            string url = $"/api/posts/{postId}/like";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UnlikePost_ShouldThrowNotFoundException_WhenPostDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 3;
            string url = $"/api/posts/{postId}/like";

            // Act
            var response = await Client.DeleteAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Post not found");
        }

        [Fact]
        public async Task UnlikePost_ShouldThrowNotFoundException_WhenPostIsNotLikedByUser()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 2;
            string url = $"/api/posts/{postId}/like";

            // Act
            var response = await Client.DeleteAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Posts like not found");
        }
    }
}
