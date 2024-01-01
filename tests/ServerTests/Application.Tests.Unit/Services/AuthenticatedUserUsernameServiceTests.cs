using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using Application.Tests.Unit.TestUtilities;
using Domain.Entities;

namespace Application.Tests.Unit.Services
{
    public class AuthenticatedUserUsernameServiceTests
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IApplicationDbContext _context;
        private readonly AuthenticatedUserUsernameService _service;

        public AuthenticatedUserUsernameServiceTests()
        {
            _authenticatedUserService = Substitute.For<IAuthenticatedUserService>();
            _context = TestBase.CreateTestDbContext();
            _service = new AuthenticatedUserUsernameService(_authenticatedUserService, _context);
        }

        [Fact]
        public async Task GetUsernameByIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistentUserId = 111;
            _authenticatedUserService.UserId.Returns(nonExistentUserId);

            // Act
            var result = async () => await _service.GetUsernameByIdAsync();

            // Assert
            await result.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Authenticated user not found from JWT.");
        }

        [Fact]
        public async Task GetUsernameByIdAsync_ShouldReturnCorrectUsername_WhenUserExists()
        {
            // Arrange
            var testUserId = 1;
            var testUser = new AppUser { Id = testUserId, UserName = "FakeTestUser" };
            _context.Users.Add(testUser);
            await _context.SaveChangesAsync(CancellationToken.None);
            _authenticatedUserService.UserId.Returns(testUserId);

            // Act
            var result = await _service.GetUsernameByIdAsync();

            // Assert
            result.Should().Be("FakeTestUser");
        }
    }
}
