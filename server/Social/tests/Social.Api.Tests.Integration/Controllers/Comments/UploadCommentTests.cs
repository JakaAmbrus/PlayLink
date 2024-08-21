using Social.Application.Features.Comments.Common;
using Social.Application.Features.Comments.UploadComment;

namespace Social.Api.Tests.Integration.Controllers.Comments
{
    [Collection("Sequential")]
    public class UploadCommentTests : BaseIntegrationTest
    {
        public UploadCommentTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Posts.Add(new Post { PostId = 1, AppUserId = 1, CommentsCount = 0 });

            await Context.SaveChangesAsync(CancellationToken.None);
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task UploadComment_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            
            var commentUploadDto = new CommentUploadDto 
            { 
                PostId = 1,
                Content = "Test comment content"
            
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/comments", commentUploadDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UploadComment_ShouldReturnCommendDto_WhenCommentIsUploaded()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            var commentUploadDto = new CommentUploadDto
            {
                PostId = 1,
                Content = "Test comment content"

            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/comments", commentUploadDto);

            var result = await response.Content.ReadFromJsonAsync<UploadCommentResponse>();

            // Assert
            result.Should().NotBeNull();
            result.CommentDto.Should().NotBeNull();
            result.CommentDto.Should().BeOfType<CommentDto>();
            result.CommentDto.PostId.Should().Be(commentUploadDto.PostId);
            result.CommentDto.AppUserId.Should().Be(1);
            result.CommentDto.LikesCount.Should().Be(0);
            result.CommentDto.IsLikedByCurrentUser.Should().BeFalse();
            result.CommentDto.IsAuthorized.Should().BeTrue();
        }

        [Fact]
        public async Task UploadComment_ShouldIncrementCommentsCount_WhenCommentIsUploaded()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            var commentUploadDto = new CommentUploadDto
            {
                PostId = 1,
                Content = "Test comment content"

            };

            // Act
            await Client.PostAsJsonAsync("/api/comments", commentUploadDto);

            RefreshContext();

            // Assert
            var updatedPost = await Context.Posts.FindAsync(1);
            updatedPost.CommentsCount.Should().Be(1);
        }

        [Fact]
        public async Task UploadComment_ShouldTrimCommentContent_DuringUpload()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            var commentUploadDto = new CommentUploadDto
            {
                PostId = 1,
                Content = "   Test comment content   "

            };

            // Act
            await Client.PostAsJsonAsync("/api/comments", commentUploadDto);

            RefreshContext();

            // Assert
            var updatedPost = await Context.Posts.FindAsync(1);
            updatedPost.CommentsCount.Should().Be(1);
            var comment = await Context.Comments.FindAsync(1);
            comment.Content.Should().Be("Test comment content");
        }

        [Fact]
        public async Task UploadComment_ShouldThrowForbiddenStatusCode_WhenAuthUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            var commentUploadDto = new CommentUploadDto
            {
                PostId = 1,
                Content = "Test comment content"
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/comments", commentUploadDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UploadComment_ShouldThrowNotFoundException_WhenPostIsNotFound()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            var commentUploadDto = new CommentUploadDto
            {
                PostId = 2,
                Content = "Test comment content"
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/comments", commentUploadDto);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Post not found");
        }

        [Fact]
        public async Task UploadComment_ShouldThrowNotFoundException_WhenAuthorizedUserIsNotFound()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            await Context.Users.AddAsync(new AppUser { Id = 2, UserName = "testertwo" });
            await Context.Posts.AddAsync(new Post { PostId = 2, AppUserId = 2, CommentsCount = 0 });
            var user = await Context.Users.FindAsync(1);
            Context.Users.Remove(user);
            await Context.SaveChangesAsync();
           
            var commentUploadDto = new CommentUploadDto
            {
                PostId = 2,
                Content = "Test comment content"
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/comments", commentUploadDto);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("User not found");
        }

        [Fact]
        public async Task UploadComment_ShouldThrowBadRequestValidationException_WhenNoPostIdIsProvided()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            var commentUploadDto = new CommentUploadDto
            {
                Content = "Test comment content"
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/comments", commentUploadDto);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Errors["Comment.PostId"].Should().HaveCount(1);
            errorResponse.Errors["Comment.PostId"][0].Should().Be("Post Id required.");
        }

        [Fact]
        public async Task UploadComment_ShouldThrowBadRequestValidationException_WhenCommentContentIsNull()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            var commentUploadDto = new CommentUploadDto
            {
                PostId = 1,
                Content = null
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/comments", commentUploadDto);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Errors["Comment.Content"].Should().HaveCount(1);
            errorResponse.Errors["Comment.Content"][0].Should().Be("Comment content is required.");
        }

        [Fact]
        public async Task UploadComment_ShouldThrowBadRequestValidationException_WhenCommentContentIsTooLong()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            var commentUploadDto = new CommentUploadDto
            {
                PostId = 1,
                Content = new string('*', 501)
            };

            // Act
            var response = await Client.PostAsJsonAsync("/api/comments", commentUploadDto);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Errors["Comment.Content"].Should().HaveCount(1);
            errorResponse.Errors["Comment.Content"][0].Should().Be("Comment content must not exceed 300 characters.");
        }
    }
}
