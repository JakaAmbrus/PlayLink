using Social.Application.Features.Posts.Common;
using Social.Application.Features.Posts.GetPostById;

namespace Social.Api.Tests.Integration.Controllers.Posts
{
    [Collection("Sequential")]
    public class GetPostByIdTests : BaseIntegrationTest
    {
        public GetPostByIdTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Users.Add(new AppUser { Id = 2, UserName = "testertwo" });

            Context.Posts.Add(new Post { PostId = 1, AppUserId = 1 });
            Context.Posts.Add(new Post { PostId = 2, AppUserId = 2 });

            await Context.SaveChangesAsync(CancellationToken.None);
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task GetPostById_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPostById_ShouldReturnPostDTO_WhenCalled()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetPostByIdResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Post.Should().BeOfType<PostDto>();
            result.Post.PostId.Should().Be(postId);
        }

        [Fact]
        public async Task GetPostById_ShouldAuthorizedTrue_WhenUserIsOwner()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetPostByIdResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Post.IsAuthorized.Should().BeTrue();
        }

        [Fact]
        public async Task GetPostById_ShouldAuthorizedTrue_WhenUserIsModerator()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator" });

            int postId = 2;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetPostByIdResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Post.IsAuthorized.Should().BeTrue();
        }

        [Fact]
        public async Task GetPostById_ShouldAuthorizedFalse_WhenUserIsNotOwnerOrModerator()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 2;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetPostByIdResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Post.IsAuthorized.Should().BeFalse();
        }

        [Fact]
        public async Task GetPostById_ShouldThrowForbiddenStatusCode_WhenAuthUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            int postId = 1;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetPostById_ShouldThrowNotFoundException_WhenPostDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 3;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.GetAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Post not found");
        }

        [Fact]
        public async Task GetPostById_ShouldThrowBadRequestValidationException_WhenTheRolesAreInvalid()
        {
            // Arrange
            await ResetDatabaseAsync();
            await RoleManager.CreateAsync(new AppRole { Name = "InvalidRole" });
            await InitializeAuthenticatedClient(new List<string> { "Member", "InvalidRole" });
            await InitializeTestSeedDataAsync();

            int postId = 1;
            string url = $"/api/posts/{postId}";

            // Act
            var response = await Client.GetAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Errors.Should().HaveCount(1);
            errorResponse.Errors["AuthUserRoles"].Should().HaveCount(1);
            errorResponse.Errors["AuthUserRoles"][0].Should().Be("Invalid user role.");
        }
    }
}
