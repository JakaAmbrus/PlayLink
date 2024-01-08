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

        [Fact]
        public async Task DeleteComment_ShouldThrowUnauthorizedException_WhenUserIsNotTheOwnerAndNotModerator()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 2;
            string url = $"/api/comments/{commentId}";

            // Act
            var response = await Client.DeleteAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            result["message"].Value<string>().Should().Be("User not authorized to delete comment");
        }

        [Fact]
        public async Task DeleteComment_ShouldThrowNotFoundException_WhenCommentIsNotFound()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 3;
            string url = $"/api/comments/{commentId}";

            // Act
            var response = await Client.DeleteAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result["message"].Value<string>().Should().Be("Comment was not found");
        }

        [Fact]
        public async Task GetPostComments_ShouldThrowForbiddenStatusCode_WhenUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            int postId = 1;
            string url = $"/api/comments/{postId}";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteComment_ShouldThrowBadRequestValidationExceptionWhen_WhenTheRolesAreInvalid()
        {
            // Arrange
            await ResetDatabaseAsync();
            await RoleManager.CreateAsync(new AppRole { Name = "InvalidRole" });
            await InitializeAuthenticatedClient(new List<string> { "Member", "InvalidRole" });
            await InitializeTestSeedDataAsync();

            int commentId = 1;
            string url = $"/api/comments/{commentId}";

            // Act
            var response = await Client.DeleteAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result["errors"]["AuthUserRoles"].Should().HaveCount(1);
            result["errors"]["AuthUserRoles"][0].Value<string>().Should().Be("Invalid role detected.");
        }
    }
}
