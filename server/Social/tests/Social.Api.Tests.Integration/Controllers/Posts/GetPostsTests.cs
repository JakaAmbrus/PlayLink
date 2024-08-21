using Social.Application.Features.Posts.Common;

namespace Social.Api.Tests.Integration.Controllers.Posts
{
    [Collection("Sequential")]
    public class GetPostsTests : BaseIntegrationTest
    {
        public GetPostsTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Users.Add(new AppUser { Id = 2, UserName = "testertwo" });

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
        public async Task GetPosts_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            // Act
            var response = await Client.GetAsync("/api/posts?pageNumber=1&pageSize=5");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPosts_ShouldReturnPagedListOfPostDTOs_WhenPostsExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            // Act
            var response = await Client.GetAsync("/api/posts?pageNumber=1&pageSize=5");
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Posts.Should().NotBeNull();
            result.Posts.Count.Should().Be(5);
            result.Posts.Should().AllBeOfType<PostDto>();
            result.Posts.Should().BeInDescendingOrder<DateTime>(p => p.DatePosted);
        }

        [Fact]
        public async Task GetPosts_ShouldReturnPagedListOfPostDTOs_WhenThereAreLessPostsThanPageSize()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            Context.Posts.RemoveRange(Context.Posts);
            Context.Posts.Add(new Post
            {
                PostId = 1,
                AppUserId = 1,
                Description = "Description",
                PhotoPublicId = "publicId",
                PhotoUrl = "url",
                DatePosted = DateTime.UtcNow.AddMinutes(-1),
                CommentsCount = 0,
                LikesCount = 0,
            });
            await Context.SaveChangesAsync(CancellationToken.None);

            // Act
            var response = await Client.GetAsync("/api/posts?pageNumber=1&pageSize=15");
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Posts.Should().NotBeNull();
            result.Posts.Count.Should().Be(1);
        }

        [Fact]
        public async Task GetPosts_ShouldReturnMaxNumberOfPostDTOs_WhenPageSizeIsGreaterThanMaxPageSize()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            // Act
            var response = await Client.GetAsync("/api/posts?pageNumber=1&pageSize=100");
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Posts.Should().NotBeNull();
            result.Posts.Count.Should().Be(20);
        }

        [Fact]
        public async Task GetPosts_ShouldReturnAuthorizedTrue_WhenUserIsOwnerOfPost()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            // Act
            var response = await Client.GetAsync("/api/posts?pageNumber=1&pageSize=5");
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            result.Posts[0].IsAuthorized.Should().BeTrue();
        }

        [Fact]
        public async Task GetPosts_ShouldReturnAuthorizedTrueOnAllPosts_WhenUserIsModerator()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator" });

            // Act
            var response = await Client.GetAsync("/api/posts?pageNumber=1&pageSize=5");
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            result.Posts.Should().Match(p => p.All(post => post.IsAuthorized == true));
        }

        [Fact]
        public async Task GetPosts_ShouldReturnAuthorizedFalse_WhenUserIsNotOwnerOfPost()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            // Act
            var response = await Client.GetAsync("/api/posts?pageNumber=1&pageSize=15");
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            var post = result.Posts.Find(p => p.PostId == 11);
            post.IsAuthorized.Should().BeFalse();
        }

        [Fact]
        public async Task GetPosts_ShouldReturnAnEmptyList_WhenThereAreNoPosts()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            Context.Posts.RemoveRange(Context.Posts);
            await Context.SaveChangesAsync(CancellationToken.None);

            // Act
            var response = await Client.GetAsync("/api/posts?pageNumber=1&pageSize=5");
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Posts.Should().NotBeNull();
            result.Posts.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetPosts_ShouldReturnAnEmptyList_WhenPageNumberIsGreaterThanTotalPages()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            // Act
            var response = await Client.GetAsync("/api/posts?pageNumber=3&pageSize=15");
            var result = await response.Content.ReadFromJsonAsync<GetPostsResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Posts.Should().NotBeNull();
            result.Posts.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetPosts_ShouldThrowForbiddenStatusCode_WhenAuthUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            // Act
            var response = await Client.GetAsync("/api/posts?pageNumber=1&pageSize=5");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetPosts_ShouldThrowBadRequestValidationException_WhenTheRolesAreInvalid()
        {
            // Arrange
            await ResetDatabaseAsync();
            await RoleManager.CreateAsync(new AppRole { Name = "InvalidRole" });
            await InitializeAuthenticatedClient(new List<string> { "Member", "InvalidRole" });
            await InitializeTestSeedDataAsync();

            // Act
            var response = await Client.GetAsync("/api/posts?pageNumber=1&pageSize=5");
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
        public async Task GetPosts_ShouldThrowBadRequestValidationException_WhenPageSizeIsLessThanOne()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            // Act
            var response = await Client.GetAsync("/api/posts?pageNumber=1&pageSize=0");
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Errors.Should().HaveCount(1);
            errorResponse.Errors["Params.PageSize"].Should().HaveCount(1);
            errorResponse.Errors["Params.PageSize"][0].Should().Be("Page Size must be greater than 0.");
        }

        [Fact]
        public async Task GetPosts_ShouldThrowBadRequestValidationException_WhenPageNumberIsLessThanOne()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            // Act
            var response = await Client.GetAsync("/api/posts?pageNumber=0&pageSize=5");
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Errors.Should().HaveCount(1);
            errorResponse.Errors["Params.PageNumber"].Should().HaveCount(1);
            errorResponse.Errors["Params.PageNumber"][0].Should().Be("Page Number must be greater than 0.");
        }
    }
}
