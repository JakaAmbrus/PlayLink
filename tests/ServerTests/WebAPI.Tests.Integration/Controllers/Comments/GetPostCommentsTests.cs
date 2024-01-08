namespace WebAPI.Tests.Integration.Controllers.Comments
{
    [Collection("Sequential")]
    public class GetPostCommentsTests : BaseIntegrationTest
    {
        public GetPostCommentsTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
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

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/comments/{postId}";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnPagedListOfCommentDTOs_WhenPostHasComments()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
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
            await InitializeTestAsync(new List<string> { "Member" });

            var postId = 2;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            result.Should().NotBeNull();
            result["comments"].Should().BeNull();
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnPagedListOfCommentDTOs_WhenPostHasLessCommentsThanPageSize()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            var postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=15";

            // Act
            var response = await Client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            result.Should().NotBeNull();
            result["comments"].Should().HaveCount(10);
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnMaxNumberOfCommentDtos_WhenPageSizeIsGreaterThanMaxPageSize()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            var comments = Enumerable.Range(11, 20)
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

            var postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=100";

            // Act
            var response = await Client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            result.Should().NotBeNull();
            result["comments"].Should().HaveCount(20);
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnAnEmptyList_WhenPageNumberIsGreaterThanTotalPages()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=3&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            result.Should().NotBeNull();
            result["comments"].Should().HaveCount(0);
        }

        [Fact]
        public async Task GetPostComments_ShouldThrowNotFoundException_WhenPostDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 3;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ApiException>(content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            errorResponse.Message.Should().Be("Post not found");
        }

        [Fact]
        public async Task GetPostComments_ShouldThrowForbiddenStatusCode_WhenAuthUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            int postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetPostComments_ShouldThrowBadRequestValidationException_WhenTheRolesAreInvalid()
        {
            // Arrange
            await ResetDatabaseAsync();
            await RoleManager.CreateAsync(new AppRole { Name = "InvalidRole" });
            await InitializeAuthenticatedClient(new List<string> { "Member", "InvalidRole" });
            await InitializeTestSeedDataAsync();

            int postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result["errors"]["AuthUserRoles"].Should().HaveCount(1);
            result["errors"]["AuthUserRoles"][0].Value<string>().Should().Be("Invalid role detected.");
        }

        [Fact]
        public async Task GetPostComments_ShouldThrowBadRequestValidationExceptionWhenPageSizeIsZero()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=0";

            // Act
            var response = await Client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result["errors"]["Params.PageSize"].Should().HaveCount(1);
            result["errors"]["Params.PageSize"][0].Value<string>().Should().Be("Page Size must be greater than 0.");
        }

        [Fact]
        public async Task GetPostComments_ShouldThrowBadRequestValidationExceptionWhenPageSizeIsLessThanZero()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=-1";

            // Act
            var response = await Client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result["errors"]["Params.PageSize"].Should().HaveCount(1);
            result["errors"]["Params.PageSize"][0].Value<string>().Should().Be("Page Size must be greater than 0.");
        }

        [Fact]
        public async Task GetPostComments_ShouldThrowBadRequestValidationExceptionWhenPageNumberIsZero()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=0&pageSize=10";

            // Act
            var response = await Client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result["errors"]["Params.PageNumber"].Should().HaveCount(1);
            result["errors"]["Params.PageNumber"][0].Value<string>().Should().Be("Page Number must be greater than 0.");
        }

        [Fact]
        public async Task GetPostComments_ShouldThrowBadRequestValidationExceptionWhenPageNumberIsLessThanZero()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=-1&pageSize=10";

            // Act
            var response = await Client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(responseString);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Should().NotBeNull();
            result["errors"]["Params.PageNumber"].Should().HaveCount(1);
            result["errors"]["Params.PageNumber"][0].Value<string>().Should().Be("Page Number must be greater than 0.");
        }
    }
}
