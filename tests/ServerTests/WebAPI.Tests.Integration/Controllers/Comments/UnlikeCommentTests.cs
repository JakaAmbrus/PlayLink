using Application.Features.Likes.UnlikeComment;

namespace WebAPI.Tests.Integration.Controllers.Comments
{
    public class UnlikeCommentTests : BaseIntegrationTest
    {
        public UnlikeCommentTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Posts.Add(new Post { PostId = 1, AppUserId = 1, CommentsCount = 2 });

            Context.Comments.Add(new Comment { CommentId = 1, PostId = 1, AppUserId = 1, LikesCount = 0 });
            Context.Comments.Add(new Comment { CommentId = 2, PostId = 1, AppUserId = 1, LikesCount = 1 });

            Context.Likes.Add(new Like { AppUserId = 1, CommentId = 2 });

            await Context.SaveChangesAsync(CancellationToken.None);
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task UnlikeComment_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 2;
            string url = $"/api/comments/{commentId}/like";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Fact]
        public async Task UnlikeComment_ShouldRemoveLikeFromComment_WhenCommentExistsAndIsLikedByUser()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 2;
            string url = $"/api/comments/{commentId}/like";

            // Act
            await Client.DeleteAsync(url);

            // Assert
            Context.Likes.Count().Should().Be(0);
        }

        [Fact]
        public async Task UnlikeComment_ShouldDecrementLikesCount_WhenCommentExistsAndIsLikedByUser()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 2;
            string url = $"/api/comments/{commentId}/like";

            // Act
            await Client.DeleteAsync(url);

            RefreshContext();

            // Assert
            Context.Comments.Find(commentId).LikesCount.Should().Be(0);
        }

        [Fact]
        public async Task UnlikeComment_ShouldReturnUnlikedTrue_WhenLikeIsRemoved()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 2;
            string url = $"/api/comments/{commentId}/like";

            // Act
            var response = await Client.DeleteAsync(url);

            var result = await response.Content.ReadFromJsonAsync<UnlikeCommentResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Unliked.Should().Be(true);
        }

        [Fact]
        public async Task UnlikeComment_ShouldThrowForbiddenStatusCode_WhenAuthUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            int commentId = 1;
            string url = $"/api/comments/{commentId}/like";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UnlikeComment_ShouldThrowNotFoundException_WhenCommentDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 3;
            string url = $"/api/comments/{commentId}/like";

            // Act
            var response = await Client.DeleteAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Comment not found");
        }

        [Fact]
        public async Task UnlikeComment_ShouldThrowNotFoundException_WhenLikeDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 1;
            string url = $"/api/comments/{commentId}/like";

            // Act
            var response = await Client.DeleteAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Comments like not found");
        }     
    }
}
