using Social.Application.Features.Admin.AdminEditRoles;

namespace Social.Api.Tests.Integration.Controllers.Admin
{
    public class EditRolesTests : BaseIntegrationTest
    {
        public EditRolesTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            var member = new AppUser { Id = 2, UserName = "member" };
            var moderator = new AppUser { Id = 3, UserName = "moderator" };

            await UserManager.CreateAsync(member, "Password123");
            await UserManager.CreateAsync(moderator, "Password123");

            await UserManager.AddToRoleAsync(await UserManager.FindByUsernameAsync("member"), "Member");
            await UserManager.AddToRoleAsync(await UserManager.FindByUsernameAsync("moderator"), "Moderator");
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task EditRoles_ShouldEditRolesAndReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            int userId = 1;
            string url = $"/api/admin/edit-roles/{userId}";

            // Act
            var response = await Client.PutAsync(url, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task EditRoles_ShouldAddModeratorRoleAndReturnTrue_WhenUserIsNotModerator()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            int userId = 2;
            string url = $"/api/admin/edit-roles/{userId}";

            // Act
            var response = await Client.PutAsync(url, null);
            var result = await response.Content.ReadFromJsonAsync<AdminEditRolesResponse>();

            RefreshContext();

            // Assert
            UserManager.IsInRoleAsync(await UserManager.FindByUsernameAsync("member"), "Moderator").Result.Should().BeTrue();
            result.RoleEdited.Should().BeTrue();
        }

        [Fact]
        public async Task EditRoles_ShouldRemoveModeratorRoleAndReturnTrue_WhenUserIsModerator()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            int userId = 3;
            string url = $"/api/admin/edit-roles/{userId}";

            // Act
            var response = await Client.PutAsync(url, null);
            var result = await response.Content.ReadFromJsonAsync<AdminEditRolesResponse>();

            RefreshContext();

            // Assert
            UserManager.IsInRoleAsync(await UserManager.FindByUsernameAsync("moderator"), "Moderator").Result.Should().BeFalse();
            result.RoleEdited.Should().BeTrue();
        }

        [Fact]
        public async Task EditRoles_ShouldThrowForbiddenStatusCode_WhenUserIsNotAdmin()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator" });

            int userId = 1;
            string url = $"/api/admin/edit-roles/{userId}";

            // Act
            var response = await Client.PutAsync(url, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task EditRoles_ShouldThrowNotFoundException_WhenUserToEditDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            int userId = 4;
            string url = $"/api/admin/edit-roles/{userId}";

            // Act
            var response = await Client.PutAsync(url, null);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("User to edit not found");
        }

        [Fact]
        public async Task EditRoles_ShouldThrowNotFoundException_WhenAuthUserDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });
            var authUser = await Context.FindAsync<AppUser>(1);
            Context.Remove(authUser);
            await Context.SaveChangesAsync();

            int userId = 2;
            string url = $"/api/admin/edit-roles/{userId}";

            // Act
            var response = await Client.PutAsync(url, null);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Authenticated Admin user not found");
        }

        [Fact]
        public async Task EditRoles_ShouldThrowUnauthorizedAccessException_WhenAuthUserIsNotAdmin()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });
            var authUser = await Context.FindAsync<AppUser>(1);
            await UserManager.RemoveFromRoleAsync(authUser, "Admin");

            int userId = 2;
            string url = $"/api/admin/edit-roles/{userId}";

            // Act
            var response = await Client.PutAsync(url, null);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(401);
            errorResponse.Message.Should().Be("Unauthorized, only an Admin can make this request");
        }
    }
}
