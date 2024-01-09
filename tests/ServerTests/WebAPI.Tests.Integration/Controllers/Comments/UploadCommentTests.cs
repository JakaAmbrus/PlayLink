using Application.Features.Comments.Common;
using Application.Features.Comments.UploadComment;

namespace WebAPI.Tests.Integration.Controllers.Comments
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
    }
}
