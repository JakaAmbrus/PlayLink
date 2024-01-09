using Application.Features.Likes.LikeComment;

namespace WebAPI.Tests.Integration.Controllers.Comments
{
    [Collection("Sequential")]
    public class LikeCommentTests : BaseIntegrationTest
    {
        public LikeCommentTests(IntegrationTestWebAppFactory factory) : base(factory)
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
        public async Task LikeComment_ShouldDeleteCommentAndReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 1;
            string url = $"/api/comments/{commentId}/like";

            // Act
            var response = await Client.PostAsync(url, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task LikeComment_ShouldAddLikeToComment_WhenCommentExists()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 1;
            string url = $"/api/comments/{commentId}/like";

            // Act
            var response = await Client.PostAsync(url, null);

            RefreshContext();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Context.Comments.Find(commentId).LikesCount.Should().Be(1);
        }

        [Fact]
        public async Task LikeComment_ShouldReturnLikedTrue_WhenLikeIsAdd()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 1;
            string url = $"/api/comments/{commentId}/like";

            // Act
            var response = await Client.PostAsync(url, null);

            var result = await response.Content.ReadFromJsonAsync<LikeCommentResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Liked.Should().BeTrue();
        }

        [Fact]
        public async Task LikeComment_ShouldThrowForbiddenStatusCode_WhenAuthUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            int commentId = 1;
            string url = $"/api/comments/{commentId}/like";

            // Act
            var response = await Client.PostAsync(url, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task LikeComment_ShouldThrowNotFoundException_WhenCommentDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 3;
            string url = $"/api/comments/{commentId}/like";

            // Act
            var response = await Client.PostAsync(url, null);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Comment not found");
        }

        [Fact]
        public async Task LikeComment_ShouldThrowBadRequestException_WhenCommentIsAlreadyLikedByUser()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 2;
            string url = $"/api/comments/{commentId}/like";

            // Act
            var response = await Client.PostAsync(url, null);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Message.Should().Be("You have already liked this comment");
        }
    }
}
