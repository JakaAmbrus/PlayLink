namespace WebAPI.Tests.Integration.Controllers.Comments
{
    [Collection("Sequential")]
    public class DeleteCommentTests : BaseIntegrationTest
    {
        public DeleteCommentTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Users.Add(new AppUser { Id = 2, UserName = "testertwo" });

            Context.Posts.Add(new Post { PostId = 1, AppUserId = 1, CommentsCount = 2 });

            Context.Comments.Add(new Comment
            {
                CommentId = 1,
                PostId = 1,
                AppUserId = 1,
                Content = "Comment 1",
                TimeCommented = DateTime.UtcNow.AddDays(-1)
            });
            Context.Comments.Add(new Comment 
            { 
                CommentId = 2,
                PostId = 1,
                AppUserId = 2,
                Content = "Comment 2",
                TimeCommented = DateTime.UtcNow.AddDays(-1)
            });

            await Context.SaveChangesAsync(CancellationToken.None);
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task DeleteComment_ShouldDeleteCommentAndReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 1;
            string url = $"/api/comments/{commentId}";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteComment_ShouldDeleteCommentAndReturnTrue_WhenUserIsTheOwner()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 1;
            string url = $"/api/comments/{commentId}";

            // Act
            var response = await Client.DeleteAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result["isDeleted"].Value<bool>().Should().Be(true);
            Context.Comments.Count().Should().Be(1);
        }

        [Fact]
        public async Task DeleteComment_ShouldDeleteCommentAndReturnTrue_WhenUserIsModerator()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator" });

            int commentId = 2;
            string url = $"/api/comments/{commentId}";

            // Act
            var response = await Client.DeleteAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result["isDeleted"].Value<bool>().Should().Be(true);
            Context.Comments.Count().Should().Be(1);
        }

        [Fact]
        public async Task DeleteComment_ShouldDecrementCommentsCount_WhenCommentIsDeleted()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator" });

            int commentId1 = 1;
            string url1 = $"/api/comments/{commentId1}";

            int commentId2 = 2;
            string url2 = $"/api/comments/{commentId2}";

            // Act
            await Client.DeleteAsync(url1);
            await Client.DeleteAsync(url2);

            RefreshContext();

            // Assert
            Context.Comments.Count().Should().Be(0);
            var post = await Context.Posts.FindAsync(1);
            post.CommentsCount.Should().Be(0);
        }
    }
}
