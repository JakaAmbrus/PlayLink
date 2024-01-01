using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Tests.Unit.Services
{
    public class AuthenticatedUserServiceTests
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public AuthenticatedUserServiceTests()
        {
            _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            _authenticatedUserService = new AuthenticatedUserService(_httpContextAccessor);
        }

        [Fact]
        public void UserId_ShouldThrowInvalidOperationException_WhenUserIdIsNotValid()
        {
            // Arrange
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "invalid_id")
        };
            var identity = new ClaimsIdentity(claims);
            var user = new ClaimsPrincipal(identity);
            _httpContextAccessor.HttpContext = Substitute.For<HttpContext>();
            _httpContextAccessor.HttpContext.User.Returns(user);

            // Act
            var action = () => { var result = _authenticatedUserService.UserId; };

            // Assert
            action.Should().Throw<InvalidOperationException>()
                  .WithMessage("User ID in JWT token is invalid.");
        }

        [Fact]
        public void UserId_ShouldThrowNotFoundException_WhenUserIdIsNotPresentInTheJWT()
        {
            // Arrange
            var claims = new List<Claim>();
            var identity = new ClaimsIdentity(claims);
            var user = new ClaimsPrincipal(identity);
            _httpContextAccessor.HttpContext = Substitute.For<HttpContext>();
            _httpContextAccessor.HttpContext.User.Returns(user);

            // Act
            var action = () => { var result = _authenticatedUserService.UserId; };

            // Assert
            action.Should().Throw<NotFoundException>()
                  .WithMessage("User ID in JWT token is missing.");
        }

        [Fact]
        public void UserId_ShouldReturnCorrectUserId()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1999")
            };
            var identity = new ClaimsIdentity(claims);
            var user = new ClaimsPrincipal(identity);
            _httpContextAccessor.HttpContext = Substitute.For<HttpContext>();
            _httpContextAccessor.HttpContext.User.Returns(user);

            // Act
            var result = _authenticatedUserService.UserId;

            // Assert
            result.Should().Be(1999);
        }

        [Fact]
        public void UserRoles_ShouldReturnCorrectRoles_WhenTheyArePressentInTheJWT()
        {
            // Arrange
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Role, "Moderator"),
            new Claim(ClaimTypes.Role, "User"),
            new Claim(ClaimTypes.Role, "Guest")
        };
            var identity = new ClaimsIdentity(claims);
            var user = new ClaimsPrincipal(identity);
            _httpContextAccessor.HttpContext = Substitute.For<HttpContext>();
            _httpContextAccessor.HttpContext.User.Returns(user);

            // Act
            var result = _authenticatedUserService.UserRoles;

            // Assert
            result.Should().BeEquivalentTo( "Admin", "Moderator", "User", "Guest" );
        }

    }
}
