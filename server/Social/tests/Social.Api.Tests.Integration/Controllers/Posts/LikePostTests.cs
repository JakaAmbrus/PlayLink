using Social.Application.Features.Likes.LikePost;

namespace Social.Api.Tests.Integration.Controllers.Posts
{
    [Collection("Sequential")]
    public class LikePostTests : BaseIntegrationTest
    {
        public LikePostTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Posts.Add(new Post { PostId = 1, AppUserId = 1, LikesCount = 0 });
            Context.Posts.Add(new Post { PostId = 2, AppUserId = 1, LikesCount = 1 });

            Context.Likes.Add(new Like { PostId = 2, AppUserId = 1 });

            await Context.SaveChangesAsync();
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task LikePost_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}/like";

            // Act
            var response = await Client.PostAsync(url, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task LikePost_ShouldAddLikeToPost_WhenPostExists()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}/like";

            // Act
            await Client.PostAsync(url, null);

            RefreshContext();

            // Assert
            var post = Context.Posts.Find(postId);
            post.LikesCount.Should().Be(1);
        }

        [Fact]
        public async Task LikePost_ShouldReturnLikedTrue_WhenLikeIsAdded()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}/like";

            // Act
            var response = await Client.PostAsync(url, null);
            var result = await response.Content.ReadFromJsonAsync<LikePostResponse>();

            // Assert
            result.Liked.Should().BeTrue();
        }

        [Fact]
        public async Task LikePost_ShouldThrowForbiddenStatusCode_WhenAuthUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            int postId = 1;
            string url = $"/api/posts/{postId}/like";

            // Act
            var response = await Client.PostAsync(url, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task LikePost_ShouldThrowNotFoundException_WhenPostNotFound()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 100;
            string url = $"/api/posts/{postId}/like";

            // Act
            var response = await Client.PostAsync(url, null);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Post not found");
        }

        [Fact]
        public async Task LikePost_ShouldThrowBadRequestException_WhenPostIsAlreadyLiked()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 2;
            string url = $"/api/posts/{postId}/like";

            // Act
            var response = await Client.PostAsync(url, null);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Message.Should().Be("You have already liked this post");
        }
    }
}
