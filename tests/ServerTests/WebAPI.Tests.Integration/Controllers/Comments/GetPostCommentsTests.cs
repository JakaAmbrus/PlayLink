using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using WebAPI.Errors;
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
            Context.Posts.Add(new Post { PostId = 1, AppUserId = 1, CommentsCount = 1 });

            var comments = Enumerable.Range(1, 10)
                .Select(i => new Comment
                {
                    AppUserId = 1,
                    CommentId = i,
                    PostId = 1,
                    Content = $"Comment {i}",
                    TimeCommented = DateTime.UtcNow.AddMinutes(-i)
                }).ToList();
            Context.AddRange(comments);

            await Context.SaveChangesAsync(CancellationToken.None);
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            var postId = 1;
            var url = $"/api/comments/{postId}";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnPagedListOfCommentDTOs_WhenPostHasComments()
        {
            // Arrange
            var postId = 1;
            var url = $"/api/comments/{postId}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            result.Should().NotBeNull();
            result["comments"].Should().HaveCount(5);
            result["comments"][0]["commentId"].Value<int>().Should().Be(1);
            result["comments"][0]["postId"].Value<int>().Should().Be(1);
            result["comments"][0]["content"].Value<string>().Should().Be("Comment 1");
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnAnEmptyList_WhenPostHasNoComments()
        {
            // Arrange
            var postId = 2;
            var url = $"/api/comments/{postId}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            result.Should().NotBeNull();
            result["comments"].Should().HaveCount(0);
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnPagedListOfCommentDTOs_WhenPostHasLessCommentsThanPageSize()
        {
            // Arrange
            var postId = 1;
            var url = $"/api/comments/{postId}?pageNumber=1&pageSize=15";

            // Act
            var response = await Client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            result.Should().NotBeNull();
            result["comments"].Should().HaveCount(10);
        }

        [Fact]
        public async Task GetPostComments_ShouldThrowNotFoundException_WhenPostDoesNotExist()
        {
            // Arrange
            var postId = 3;
            var url = $"/api/comments/{postId}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ApiException>(content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            errorResponse.Message.Should().Be("Post not found");
        }
    }
}
