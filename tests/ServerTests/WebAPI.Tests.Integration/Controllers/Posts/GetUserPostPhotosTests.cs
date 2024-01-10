using Application.Features.Posts.GetUserPostPhotos;

namespace WebAPI.Tests.Integration.Controllers.Posts
{
    [Collection("Sequential")]
    public class GetUserPostPhotosTests : BaseIntegrationTest
    {
        public GetUserPostPhotosTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Users.Add(new AppUser { Id = 2, UserName = "tester" });

            var postsWithoutPhotos = Enumerable.Range(1, 10)
                .Select(i => new Post
                {
                    PostId = i,
                    AppUserId = 2,
                    Description = "Description",
                    DatePosted = DateTime.UtcNow.AddMinutes(-i),
                }).ToList();
            Context.Posts.AddRange(postsWithoutPhotos);

            var postsWithPhotos = Enumerable.Range(11, 10)
                .Select(i => new Post
                {
                    PostId = i,
                    AppUserId = 1,
                    Description = "Description",
                    PhotoPublicId = "publicId",
                    PhotoUrl = "url",
                    DatePosted = DateTime.UtcNow.AddMinutes(-(i + 10)),
                }).ToList();
            Context.Posts.AddRange(postsWithPhotos);

            await Context.SaveChangesAsync(CancellationToken.None);
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task GetUserPostPhotos_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "tester";
            string url = $"/api/posts/user/{username}/photos";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetUserPostPhotos_ShouldReturnPhotos_WhenUserHasPhotos()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "authtester";
            string url = $"/api/posts/user/{username}/photos";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetUserPostPhotosResponse>();

            // Assert
            result.Photos.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetUserPostPhotos_ShouldReturnAnEmptyList_WhenUserDoesNotHavePhotos()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "tester";
            string url = $"/api/posts/user/{username}/photos";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetUserPostPhotosResponse>();

            // Assert
            result.Photos.Should().BeEmpty();
        }

        [Fact]
        public async Task GetUserPostPhotos_ShouldReturnPostPhotosAndProfilePhoto_WhenUserHasProfilePhoto()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "authtester";
            string userPhoto = "https://CloudinaryTestPicture.com";
            string url = $"/api/posts/user/{username}/photos";

            // Act
            var response = await Client.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<GetUserPostPhotosResponse>();

            // Assert
            result.Photos.Should().HaveCount(11);
            result.Photos.Should().Contain(userPhoto);
        }

        [Fact]
        public async Task GetUserPostPhotos_ShouldReturnCorrectPhotosFromCache_WhenCacheExists()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "authtester";
            string userPhoto = "https://CloudinaryTestPicture.com";
            string url = $"/api/posts/user/{username}/photos";

            // Act 
            var response1 = await Client.GetAsync(url);
            var result1 = await response1.Content.ReadFromJsonAsync<GetUserPostPhotosResponse>();

            var userToBeModified = await Context.Users.FindAsync(1);
            userToBeModified.ProfilePictureUrl = "ChangedPictureUrl";
            await Context.SaveChangesAsync();

            var response2 = await Client.GetAsync(url);
            var result2 = await response2.Content.ReadFromJsonAsync<GetUserPostPhotosResponse>();

            // Assert 
            result1.Photos.Should().HaveCount(11);
            result1.Photos.Should().Contain(userPhoto);

            result2.Photos.Should().BeEquivalentTo(result1.Photos);
        }

        [Fact]
        public async Task GetUserPostPhotos_ShouldThrowForbiddenStatusCode_WhenAuthUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            string username = "tester";
            string url = $"/api/posts/user/{username}/photos";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetUserPostPhotos_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string username = "ImaginaryUser";
            string url = $"/api/posts/user/{username}/photos";

            // Act
            var response = await Client.GetAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("User with username ImaginaryUser does not exist");
        }
    }
}
