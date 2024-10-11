using Social.Application.Features.Posts.Common;
using Org.BouncyCastle.Asn1.Ocsp;
using Social.FunctionalTests.Configurations;
using Social.FunctionalTests.Models;

namespace Social.FunctionalTests.Controllers.Posts
{
    [Collection("Sequential")]
    public class GetPostsByUserTests : BaseIntegrationTest
    {
        public GetPostsByUserTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Users.Add(new AppUser { Id = 2, UserName = "tester" });

            var authPosts = Enumerable.Range(1, 10)
                .Select(i => new Post
                {
                    PostId = i,
                    AppUserId = 1,
                    Description = "Description",
                    PhotoPublicId = "publicId",
                    PhotoUrl = "url",
                    DatePosted = DateTime.UtcNow.AddMinutes(-i),
                    CommentsCount = 0,
                    LikesCount = 0,
                }).ToList();
            Context.Posts.AddRange(authPosts);

            var nonAuthPosts = Enumerable.Range(11, 10)
                .Select(i => new Post
                {
                    PostId = i,
                    AppUserId = 2,
                    Description = "Description",
                    PhotoPublicId = "publicId",
                    PhotoUrl = "url",
                    DatePosted = DateTime.UtcNow.AddMinutes(-(i + 10)),
                    CommentsCount = 0,
                    LikesCount = 0,
                }).ToList();
            Context.Posts.AddRange(nonAuthPosts);

            await Context.SaveChangesAsync(CancellationToken.None);
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task GetPostsByUser_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "tester";
            string url = $"/api/posts/user/{username}";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPostsByUser_ShouldReturnPagedListOfPostDTOs_WhenUserPostsExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "tester";
            string url = $"/api/posts/user/{username}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Posts.Should().NotBeNull();
            result.Posts.Count.Should().Be(5);
            result.Posts.Should().AllBeOfType<PostDto>();
        }

        [Fact]
        public async Task GetPostsByUser_ShouldReturnPagedListOfPostDTOs_WhenThereAreLessPostsThanPageSize()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "tester";
            string url = $"/api/posts/user/{username}?pageNumber=1&pageSize=20";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            result.Should().NotBeNull();
            result.Posts.Should().NotBeNull();
            result.Posts.Count.Should().Be(10);
        }

        [Fact]
        public async Task GetPostsByUser_ShouldReturnMaxNumberOfPostDTOs_WhenPageSizeIsGreaterThanMaxPageSize()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            var nonAuthPosts = Enumerable.Range(30, 10)
                .Select(i => new Post
                {
                    PostId = i,
                    AppUserId = 2,
                    Description = "Description",
                    PhotoPublicId = "publicId",
                    PhotoUrl = "url",
                    DatePosted = DateTime.UtcNow.AddMinutes(-(i + 10)),
                    CommentsCount = 0,
                    LikesCount = 0,
                }).ToList();
            Context.Posts.AddRange(nonAuthPosts);
            await Context.SaveChangesAsync(CancellationToken.None);

            string username = "tester";
            string url = $"/api/posts/user/{username}?pageNumber=1&pageSize=100";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Posts.Should().NotBeNull();
            result.Posts.Count.Should().Be(20);
        }

        [Fact]
        public async Task GetPostsByUser_ShouldReturnAuthorizedTrue_WhenOwnersPosts()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "authtester";
            string url = $"/api/posts/user/{username}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Posts.Should().Match(p => p.All(post => post.IsAuthorized == true));
        }

        [Fact]
        public async Task GetPostsByUser_ShouldReturnAuthorizedFalse_WhenNonOwnersPosts()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "tester";
            string url = $"/api/posts/user/{username}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Posts.Should().Match(p => p.All(post => post.IsAuthorized == false));
        }

        [Fact]
        public async Task GetPostsByUser_ShouldReturnAuthorizedTrue_WhenAuthUserIsModerator()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator" });

            string username = "tester";
            string url = $"/api/posts/user/{username}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Posts.Should().Match(p => p.All(post => post.IsAuthorized == true));
        }

        [Fact]
        public async Task GetPostsByUser_ShouldReturnAnEmptyList_WhenUserHasNoPosts()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            await Context.Users.AddAsync(new AppUser { Id = 3, UserName = "noposts" });
            await Context.SaveChangesAsync(CancellationToken.None);

            string username = "noposts";
            string url = $"/api/posts/user/{username}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Posts.Should().NotBeNull();
            result.Posts.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetPostsByUser_ShouldReturnAnEmptyList_WhenPageNumberIsGreaterThanTotalPages()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "tester";
            string url = $"/api/posts/user/{username}?pageNumber=3&pageSize=10";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Posts.Should().NotBeNull();
            result.Posts.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetPostsByUser_ShouldThrowForbiddenStatusCode_WhenAuthUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            string username = "tester";
            string url = $"/api/posts/user/{username}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetPostsByUser_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "notfound";
            string url = $"/api/posts/user/{username}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be($"User with Username {username} not found");
        }

        [Fact]
        public async Task GetPostsByUser_ShouldThrowBadRequestValidationException_WhenTheRolesAreInvalid()
        {
            // Arrange
            await ResetDatabaseAsync();
            await RoleManager.CreateAsync(new AppRole { Name = "InvalidRole" });
            await InitializeAuthenticatedClient(new List<string> { "Member", "InvalidRole" });
            await InitializeTestSeedDataAsync();

            string username = "tester";
            string url = $"/api/posts/user/{username}?pageNumber=1&pageSize=5";

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

        [Fact]
        public async Task GetPostsByUser_ShouldThrowBadRequestValidationException_WhenPageSizeIsLessThanOne()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "tester";
            string url = $"/api/posts/user/{username}?pageNumber=1&pageSize=0";

            // Act
            var response = await Client.GetAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Errors.Should().HaveCount(1);
            errorResponse.Errors["Params.PageSize"].Should().HaveCount(1);
            errorResponse.Errors["Params.PageSize"][0].Should().Be("Page size must be greater than 0.");
        }

        [Fact]
        public async Task GetPostsByUser_ShouldThrowBadRequestValidationException_WhenPageNumberIsLessThanOne()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "tester";
            string url = $"/api/posts/user/{username}?pageNumber=0&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Errors.Should().HaveCount(1);
            errorResponse.Errors["Params.PageNumber"].Should().HaveCount(1);
            errorResponse.Errors["Params.PageNumber"][0].Should().Be("Page number must be greater than 0.");
        }
    }
}
