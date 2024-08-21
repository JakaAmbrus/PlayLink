using Social.Application.Features.Admin.Common;

namespace Social.Api.Tests.Integration.Controllers.Admin
{
    [Collection("Sequential")]
    public class GetUsersWithRolesTests : BaseIntegrationTest
    {
        public GetUsersWithRolesTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            var members = Enumerable.Range(2, 20)
                .Select(i => new AppUser
                {
                    Id = i,
                    UserName = $"{i}",
                }).ToList();
            Context.Users.AddRange(members);

            await Context.SaveChangesAsync(CancellationToken.None);
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }

        [Fact]
        public async Task GetUsersWithRoles_ShouldDeleteCommentAndReturnCorrectResponseCode_WhenRequestIsValid()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            // Act
            var response = await Client.GetAsync("/api/admin/users");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetUsersWithRoles_ShouldReturnPagedListOfUserDTOsWithoutAdmin_WhenUsersExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            // Act
            var response = await Client.GetAsync("/api/admin/users?pageNumber=1&pageSize=5");
            var result = await response.Content.ReadFromJsonAsync<GetUsersWithRolesResponse>();

            // Assert
            result.Users.Count.Should().Be(5);
            result.Users.Should().AllBeOfType<UserWithRolesDto>();
        }

        [Fact]
        public async Task GetUsersWithRoles_ShouldReturnPagedListOfUserDTOs_WhenPageSizeIsGreaterThanNumberOfUsers()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            // Act
            var response = await Client.GetAsync("/api/admin/users?pageNumber=1&pageSize=25");
            var result = await response.Content.ReadFromJsonAsync<GetUsersWithRolesResponse>();

            // Assert
            result.Users.Count.Should().Be(20);
            result.Users.Should().AllBeOfType<UserWithRolesDto>();
        }
        [Fact]
        public async Task GetUsersWithRoles_ShouldReturnMaxNumberOfUserDTOs_WhenPageSizeIsGreaterThanMaxPageSize()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });
            var users = Enumerable.Range(25, 20)
                .Select(i => new AppUser
                {
                    Id = i,
                    UserName = $"{i}",
                }).ToList();
            Context.Users.AddRange(users);
            await Context.SaveChangesAsync(CancellationToken.None);

            // Act
            var response = await Client.GetAsync("/api/admin/users?pageNumber=1&pageSize=100");
            var result = await response.Content.ReadFromJsonAsync<GetUsersWithRolesResponse>();

            // Assert
            result.Users.Count.Should().Be(20);
            result.Users.Should().AllBeOfType<UserWithRolesDto>();
        }

        [Fact]
        public async Task GetUsersWithRoles_ShouldReturnEmptyList_WhenPageNumberIsGreaterThanNumberOfPages()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            // Act
            var response = await Client.GetAsync("/api/admin/users?pageNumber=2&pageSize=25");
            var result = await response.Content.ReadFromJsonAsync<GetUsersWithRolesResponse>();

            // Assert
            result.Users.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetUsersWithRoles_ShouldReturnAnEmptyList_WhenThereAreNoUsersOtherThanAdmin()
        {
            // Arrange
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(new List<string> { "Member", "Moderator", "Admin" });

            // Act
            var response = await Client.GetAsync("/api/admin/users?pageNumber=1&pageSize=5");
            var result = await response.Content.ReadFromJsonAsync<GetUsersWithRolesResponse>();

            // Assert
            result.Users.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetUsersWithRoles_ShouldSetIsModeratorTrue_WhenUserIsModerator()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });
            var moderator = new AppUser
            {
                Id = 100,
                UserName = "moderator",
            };
            await UserManager.CreateAsync(moderator, "Password123");
            var createdUser = await UserManager.FindByIdAsync(moderator.Id.ToString());
            await UserManager.AddToRoleAsync(createdUser, "Moderator");

            // Act
            var response = await Client.GetAsync("/api/admin/users?pageNumber=1&pageSize=25");
            var result = await response.Content.ReadFromJsonAsync<GetUsersWithRolesResponse>();

            // Assert
            result.Users.Should().Contain(u => u.AppUserId == 100 && u.IsModerator == true);
        }

        [Fact]
        public async Task GetUsersWithRoles_ShouldThrowForbiddenStatusCode_WhenUserIsNotAdminInJWT()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator" });

            // Act
            var response = await Client.GetAsync("/api/admin/users?pageNumber=1&pageSize=25");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetUsersWithRoles_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });
            var authUser = await Context.FindAsync<AppUser>(1);
            Context.Remove(authUser);
            await Context.SaveChangesAsync();

            // Act
            var response = await Client.GetAsync("/api/admin/users?pageNumber=1&pageSize=25");
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(404);
            errorResponse.Message.Should().Be("Authorized user not found");
        }

        [Fact]
        public async Task GetUsersWithRoles_ShouldThrowUnauthorizedAccessException_WhenUserIsNotAdmin()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });
            var authUser = await Context.FindAsync<AppUser>(1);
            await UserManager.RemoveFromRoleAsync(authUser, "Admin");
            await Context.SaveChangesAsync();

            // Act
            var response = await Client.GetAsync("/api/admin/users");
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(401);
            errorResponse.Message.Should().Be("Unauthorized, only an Admin can make this request");
        }

        [Fact]
        public async Task GetUsersWithRoles_ShouldThrowBadRequestValidationException_WhenPageSizeIsZero()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            // Act
            var response = await Client.GetAsync("/api/admin/users?pageNumber=1&pageSize=0");
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
        public async Task GetUsersWithRoles_ShouldThrowBadRequestValidationException_WhenPageSizeIsLessThanZero()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            // Act
            var response = await Client.GetAsync("/api/admin/users?pageNumber=1&pageSize=-1");
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
        public async Task GetUsersWithRoles_ShouldThrowBadRequestValidationException_WhenPageNumberIsZero()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            // Act
            var response = await Client.GetAsync("/api/admin/users?pageNumber=0&pageSize=5");
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
        public async Task GetUsersWithRoles_ShouldThrowBadRequestValidationException_WhenPageNumberIsLessThanZero()
        {
            // Arrange
            await InitializeTestAsync(new List<string> { "Member", "Moderator", "Admin" });

            // Act
            var response = await Client.GetAsync("/api/admin/users?pageNumber=-1&pageSize=5");
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
