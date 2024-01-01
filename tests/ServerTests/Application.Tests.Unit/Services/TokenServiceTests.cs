using Application.Exceptions;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Tests.Unit.Services
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;
        private readonly UserManager<AppUser> _userManagerMock;

        public TokenServiceTests()
        {
            var configMock = Substitute.For<IConfiguration>();
            configMock["TokenKey"].Returns("Some Secret Key that I will be using for testing purposes and has to be at least 128 bits");

            var store = Substitute.For<IUserStore<AppUser>>();
            _userManagerMock = Substitute.For<UserManager<AppUser>>(store, null, null, null, null, null, null, null, null);

            _tokenService = new TokenService(configMock, _userManagerMock);
        }

        [Fact]
        public async Task CreateToken_ShouldThrowNotFoundException_WhenNullUserIsPassedIn()
        {
            // Act
            var action = async () => await _tokenService.CreateToken(null);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                         .WithMessage("User not found while trying to create token.");
        }

        [Fact]
        public async Task CreateToken_ShouldReturnValidToken_WhenUserIsValid()
        {
            // Arrange
            var user = new AppUser { Id = 1, UserName = "TestUser" };
            _userManagerMock.GetRolesAsync(user).Returns(Task.FromResult<IList<string>>(new List<string> { "Role1", "Role2" }));

            // Act
            var result = await _tokenService.CreateToken(user);

            // Assert
            result.Should().NotBeNull();
            result.Should().Contain(".");
            result.Split('.').Length.Should().Be(3);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(result) as JwtSecurityToken;

            token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value.Should().Be(user.Id.ToString());
            token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value.Should().Be(user.UserName);
            token.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddDays(7), TimeSpan.FromSeconds(30));
            token.SignatureAlgorithm.Should().Be("HS512");

        }
    }
}
