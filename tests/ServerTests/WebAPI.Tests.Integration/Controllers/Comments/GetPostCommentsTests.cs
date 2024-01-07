using Application.Models;
using System.Net;
using WebAPI.Tests.Integration.Configurations;

namespace WebAPI.Tests.Integration.Controllers.Comments
{
    public class GetPostCommentsTests : BaseIntegrationTest
    {
        public GetPostCommentsTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
            Initialize().Wait();
        }

        private async Task Initialize()
        {
            await InitializeAuthenticatedClient();
            await InitializeSeedTestDataAsync();
        }

        private async Task InitializeSeedTestDataAsync()
        {
            Context.Posts.Add(new Post { PostId = 1, AppUserId = 1, CommentsCount = 1, PhotoUrl = "https://sdfsdf.com", PhotoPublicId = "222" });
            Context.Comments.Add(new Comment { CommentId = 1, PostId = 1, AppUserId = 1 });
            await Context.SaveChangesAsync(CancellationToken.None);
        }

        [Fact]
        public async Task DeleteComment_ShouldDeleteCommentAndReturnTheCorrectResponse_WhenUserIsTheOwner()
        {
            // Arrange
            PhotoService.DeletePhotoAsync(Arg.Any<string>())
                .Returns(Task.FromResult(new PhotoDeletionResult { Error = null }));

            var postId = 1;
            var url = $"/api/posts/{postId}";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
