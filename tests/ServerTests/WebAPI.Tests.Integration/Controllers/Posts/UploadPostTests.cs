using Application.Features.Posts.Common;
using Application.Features.Posts.UploadPost;
using Application.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace WebAPI.Tests.Integration.Controllers.Posts
{
    [Collection("Sequential")]
    public class UploadPostTests : BaseIntegrationTest
    {
        private const string TestPhotoFieldName = "PhotoFile";
        private const string TestPhotoFileName = "test.jpg";
        private const string TestDescriptionFieldName = "Description";
        private const string TestPhotoUrl = "http://test.com/photo.jpg";
        public UploadPostTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
        }

        private void MockPhotoUpload()
        {
            var mockPhotoResult = new PhotoUploadResult
            {
                PublicId = "test_public_id",
                Url = TestPhotoUrl,
                Error = null
            };

            PhotoService.AddPhotoAsync(Arg.Any<IFormFile>(), Arg.Any<string>())
                .Returns(Task.FromResult(mockPhotoResult));
        }

        private ByteArrayContent CreateMockFileContent()
        {
            var fileContent = new ByteArrayContent(new byte[32]);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = $"\"{TestPhotoFieldName}\"",
                FileName = $"\"{TestPhotoFileName}\""
            };

            return fileContent;
        }

        [Fact]
        public async Task UploadPost_ShouldReturnCorrectResponseCode_WhenRequestIsValidWithoutPhoto()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            var postContent = new MultipartFormDataContent
            {
                { new StringContent("Test description"), TestDescriptionFieldName },
            };

            // Act
            var response = await Client.PostAsync("/api/posts", postContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UploadPost_ShouldReturnCorrectResponseCode_WhenRequestIsValidWithPhoto()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            MockPhotoUpload();

            var postContent = new MultipartFormDataContent
            {
                { new StringContent("Test description"), TestDescriptionFieldName },
                { CreateMockFileContent(), TestPhotoFieldName, TestPhotoFileName }
            };

            // Act
            var response = await Client.PostAsync("/api/posts", postContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UploadPost_ShouldCreatePostWithoutPhoto_WhenPhotoIsNotProvided()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            var postContent = new MultipartFormDataContent
            {
                { new StringContent("Test description"), TestDescriptionFieldName },
            };

            // Act
            var response = await Client.PostAsync("/api/posts", postContent);
            var result = await response.Content.ReadFromJsonAsync<UploadPostResponse>();

            // Assert
            result.Should().NotBeNull();
            result.PostDto.Should().BeOfType<PostDto>();
            result.PostDto.Description.Should().Be("Test description");
            result.PostDto.PhotoUrl.Should().BeNull();
        }

        [Fact]
        public async Task UploadPost_ShouldCreatePostWithPhoto_WhenPhotoIsProvided()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            MockPhotoUpload();

            var postContent = new MultipartFormDataContent
            {
                { new StringContent("Test description"), TestDescriptionFieldName },
                { CreateMockFileContent(), TestPhotoFieldName, TestPhotoFileName }
            };

            // Act
            var response = await Client.PostAsync("/api/posts", postContent);
            var result = await response.Content.ReadFromJsonAsync<UploadPostResponse>();

            // Assert
            result.Should().NotBeNull();
            result.PostDto.Should().BeOfType<PostDto>();
            result.PostDto.Description.Should().Be("Test description");
            result.PostDto.PhotoUrl.Should().Be(TestPhotoUrl);
        }

        [Fact]
        public async Task UploadPost_ShouldCreatePostWithPhoto_WhenOnlyPhotoIsProvided()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            MockPhotoUpload();

            var postContent = new MultipartFormDataContent
            {
                { CreateMockFileContent(), TestPhotoFieldName, TestPhotoFileName }
            };

            // Act
            var response = await Client.PostAsync("/api/posts", postContent);
            var result = await response.Content.ReadFromJsonAsync<UploadPostResponse>();

            // Assert
            result.Should().NotBeNull();
            result.PostDto.Should().BeOfType<PostDto>();
            result.PostDto.Description.Should().BeNull();
            result.PostDto.PhotoUrl.Should().Be(TestPhotoUrl);
        }

        [Fact]
        public async Task UploadPost_ShouldThrowForbiddenStatusCode_WhenUserIsNotMember()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { });

            var postContent = new MultipartFormDataContent
            {
                { new StringContent("Test description"), TestDescriptionFieldName },
            };

            // Act
            var response = await Client.PostAsync("/api/posts", postContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UploadPost_ShouldThrowUnauthorizedException_WhenUserDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });
            var authUsr = await Context.Users.FindAsync(1);
            Context.Remove(authUsr);
            await Context.SaveChangesAsync();

            var postContent = new MultipartFormDataContent
            {
                { new StringContent("Test description"), TestDescriptionFieldName },
            };

            // Act
            var response = await Client.PostAsync("/api/posts", postContent);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Authenticated user not found");
        }

    }
}
