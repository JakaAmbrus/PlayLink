using Application.Features.Moderator.DeleteUserDescription;

namespace WebAPI.Tests.Integration.Controllers.Moderator
{
    [Collection("Sequential")]
    public class DeleteUserDescriptionTests : BaseIntegrationTest
    {
        public DeleteUserDescriptionTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Users.Add(new AppUser { Id = 2, UserName = "Description", Description = "Test Description" });
            Context.Users.Add(new AppUser { Id = 3, UserName = "NoDescription" });

            await Context.SaveChangesAsync();
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task DeleteUserDescription_ShouldReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator" });

            string url = "/api/moderator/delete-user-description/Description";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteUserDescription_ShouldDeleteUserDescriptionAndReturnTheCorrectResponse_WhenUserIsModerator()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator" });

            string url = "/api/moderator/delete-user-description/Description";

            // Act
            var response = await Client.DeleteAsync(url);
            var result = await response.Content.ReadFromJsonAsync<DeleteUserDescriptionResponse>();

            RefreshContext();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.IsDeleted.Should().BeTrue();
            Context.Users.Should().NotContain(u => u.Description == "Test Description");
        }

        [Fact]
        public async Task DeleteUserDescription_ShouldReturnFalse_WhenUserDoesNotHaveDescription()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator" });

            string url = "/api/moderator/delete-user-description/NoDescription";

            // Act
            var response = await Client.DeleteAsync(url);
            var result = await response.Content.ReadFromJsonAsync<DeleteUserDescriptionResponse>();

            RefreshContext();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteUserDescription_ShouldThrowForbiddenStatusCode_WhenUserIsNotModerator()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member" });

            string url = "/api/moderator/delete-user-description/NoDescription";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteUserDescription_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator" });

            string url = "/api/moderator/delete-user-description/Imaginary";

            // Act
            var response = await Client.DeleteAsync(url);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Contain("User not found.");
        }
    }
}
