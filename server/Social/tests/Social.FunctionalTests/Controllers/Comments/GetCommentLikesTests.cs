using Social.Application.Features.Likes.Common;
using Social.Application.Features.Likes.GetCommentLikes;
using Social.FunctionalTests.Configurations;

namespace Social.FunctionalTests.Controllers.Comments
{
    [Collection("Sequential")]
    public class GetCommentLikesTests : BaseIntegrationTest
    {
        public GetCommentLikesTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        private async Task InitializeTestSeedDataAsync()
        {
            var users = Enumerable.Range(2, 10)
                .Select(i => new AppUser
                {
                    Id = i,
                    UserName = $"{i}",
                    FullName = $"{i} Tester",
                }).ToList();
            Context.Users.AddRange(users);

            Context.Posts.Add(new Post { PostId = 1, AppUserId = 1, CommentsCount = 0 });

            Context.Comments.Add(new Comment { CommentId = 1, PostId = 1, AppUserId = 1, LikesCount = 11 });
            Context.Comments.Add(new Comment { CommentId = 2, PostId = 1, AppUserId = 1, LikesCount = 0 });

            var likes = Enumerable.Range(1, 11)
                .Select(i => new Like
                {
                    AppUserId = i,
                    CommentId = 1,
                }).ToList();
            Context.Likes.AddRange(likes);

            await Context.SaveChangesAsync(CancellationToken.None);
        }

        [Fact]
        public async Task GetCommentLikes_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 1;
            string url = $"/api/comments/{commentId}/likes";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetCommentLikes_ShouldReturnUsersThatLikedCommentDtoExcludingCurrent_WhenCommentExists()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 1;
            string url = $"/api/comments/{commentId}/likes";

            // Act
            var response = await Client.GetAsync(url);

            var result = await response.Content.ReadFromJsonAsync<GetCommentLikesResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.LikedUsers.Should().NotBeNull();
            result.LikedUsers.Should().AllBeOfType<LikedUserDto>();
            result.LikedUsers.Count.Should().Be(10);
            result.LikedUsers[0].FullName.Should().Be("2 Tester");
            result.LikedUsers[0].Username.Should().Be("2");
        }

        [Fact]
        public async Task GetCommentLikes_ShouldReturnEmptyList_WhenCommentHasNoLikes()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 2;
            string url = $"/api/comments/{commentId}/likes";

            // Act
            var response = await Client.GetAsync(url);

            var result = await response.Content.ReadFromJsonAsync<GetCommentLikesResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.LikedUsers.Should().NotBeNull();
            result.LikedUsers.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCommentLikes_ShouldThrowForbiddenStatusCode_WhenAuthUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            int commentId = 1;
            string url = $"/api/comments/{commentId}/likes";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

       [Fact]
        public async Task GetCommentLikes_ShouldThrowNotFoundException_WhenCommentDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int commentId = 3;
            string url = $"/api/comments/{commentId}/likes";

            // Act
            var response = await Client.GetAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Comment not found");
        }
    }
}
