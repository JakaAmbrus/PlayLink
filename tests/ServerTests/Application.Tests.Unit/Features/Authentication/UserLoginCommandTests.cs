using Application.Exceptions;
using Application.Features.Authentication.Common;
using Application.Features.Authentication.UserLogin;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using MediatR;

namespace Application.Tests.Unit.Features.Authentication
{
    public class UserLoginCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IUserManager _userManager;
        private readonly ITokenService _tokenService;

        public UserLoginCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            _userManager = Substitute.For<IUserManager>();
            _tokenService = Substitute.For<ITokenService>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<UserLoginCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new UserLoginCommandHandler(_userManager, _tokenService)
                .Handle(c.Arg<UserLoginCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1, UserName = "tester" });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task UserLogin_ShouldReturnMemberUserDTO_WhenInputsAreValid()
        {
            // Arrange
            _userManager.FindByUsernameAsync(Arg.Any<string>()).Returns(Task.FromResult(new AppUser() { UserName = "tester"}));
            _userManager.CheckPasswordAsync(Arg.Any<AppUser>(), Arg.Any<string>()).Returns(Task.FromResult(true));
            _tokenService.CreateToken(Arg.Any<AppUser>()).Returns("token");

            var request = new UserLoginCommand
            {
                Username = "tester",
                Password = "Password1"
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<UserLoginResponse>();
            response.User.Should().BeOfType<UserDto>();
            response.User.Token.Should().Be("token");
            response.User.Username.Should().Be("tester");
        }

        [Fact]
        public async Task UserLogin_ShouldThrowUnauthorizedException_WhenPasswordIsInvalid()
        {
            // Arrange
            _userManager.FindByUsernameAsync(Arg.Any<string>()).Returns(Task.FromResult(new AppUser() { UserName = "tester" }));
            _userManager.CheckPasswordAsync(Arg.Any<AppUser>(), Arg.Any<string>()).Returns(Task.FromResult(false));

            var request = new UserLoginCommand
            {
                Username = "tester",
                Password = "Password2"
            };

            // Act
            Func<Task> act = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid Username or Password");
        }

        [Fact]
        public async Task UserLogin_ShouldThrowUnauthorizedException_WhenUserDoesNotExist()
        {
            // Arrange
            _userManager.FindByUsernameAsync(Arg.Any<string>()).Returns(Task.FromResult((AppUser)null));

            var request = new UserLoginCommand
            {
                Username = "tester2",
                Password = "Password1"
            };

            // Act
            Func<Task> act = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid Username or Password");
        }
    }
}
