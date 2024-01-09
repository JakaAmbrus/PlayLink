using Application.Features.Comments.Common;

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
            var result = await response.Content.ReadFromJsonAsync<GetCommentResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Comments.Should().AllBeOfType<CommentDto>();
            result.Comments.Should().HaveCount(5);
            result.Comments[0].CommentId.Should().Be(1);
            result.Comments[0].PostId.Should().Be(1);
            result.Comments[0].Content.Should().Be("Comment 1");
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnAnEmptyList_WhenPostHasNoComments()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            await Context.Posts.AddAsync(new Post { PostId = 2, AppUserId = 1, CommentsCount = 0 });
            await Context.SaveChangesAsync(CancellationToken.None);

            var postId = 2;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetCommentResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Comments.Should().AllBeOfType<CommentDto>();
            result.Comments.Should().HaveCount(0);
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
            var result = await response.Content.ReadFromJsonAsync<GetCommentResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Comments.Should().AllBeOfType<CommentDto>();
            result.Comments.Should().HaveCount(10);
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
            var result = await response.Content.ReadFromJsonAsync<GetCommentResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Comments.Should().AllBeOfType<CommentDto>();
            result.Comments.Should().HaveCount(20);
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnAuthorizedTrue_WhenUserIsOwnerOfComment()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            var postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetCommentResponse>();

            // Assert
            result.Comments[0].IsAuthorized.Should().BeTrue();
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnAuthorizedFalse_WhenUserIsNotOwnerOfComment()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            Context.Users.Add(new AppUser { Id = 2 });
            Context.Comments.Add(new Comment { CommentId = 11, PostId = 1, AppUserId = 2 });
            await Context.SaveChangesAsync(CancellationToken.None);

            var postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=15";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetCommentResponse>();

            // Assert
            var comment = result.Comments.Find(c => c.CommentId == 11);
            comment.IsAuthorized.Should().BeFalse();
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnAuthorizedTrueOnAllComments_WhenUserIsModerator()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator" });

            var postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=5";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetCommentResponse>();

            // Assert
            result.Comments.Should().Match(c => c.All(comment => comment.IsAuthorized == true));
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
            var result = await response.Content.ReadFromJsonAsync<GetCommentResponse>();

            // Assert
            result.Should().NotBeNull();
            result.Comments.Should().AllBeOfType<CommentDto>();
            result.Comments.Should().HaveCount(0);
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
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
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
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                errorResponse.Should().NotBeNull();
                errorResponse.StatusCode.Should().Be(400);
                errorResponse.Errors.Should().HaveCount(1);
                errorResponse.Errors["AuthUserRoles"].Should().HaveCount(1);
                errorResponse.Errors["AuthUserRoles"][0].Should().Be("Invalid role detected.");           
        }

        [Fact]
        public async Task GetPostComments_ShouldThrowBadRequestValidationException_WhenPageSizeIsZero()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=0";

            // Act
            var response = await Client.GetAsync(url);
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
        public async Task GetPostComments_ShouldThrowBadRequestValidationException_WhenPageSizeIsLessThanZero()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=1&pageSize=-1";

            // Act
            var response = await Client.GetAsync(url);
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
        public async Task GetPostComments_ShouldThrowBadRequestValidationException_WhenPageNumberIsZero()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=0&pageSize=10";

            // Act
            var response = await Client.GetAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Errors.Should().HaveCount(1);
            errorResponse.Errors["Params.PageNumber"].Should().HaveCount(1);
            errorResponse.Errors["Params.PageNumber"][0].Should().Be("Page Number must be greater than 0.");
        }

        [Fact]
        public async Task GetPostComments_ShouldThrowBadRequestValidationException_WhenPageNumberIsLessThanZero()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            int postId = 1;
            string url = $"/api/comments/{postId}?pageNumber=-1&pageSize=10";

            // Act
            var response = await Client.GetAsync(url);
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
