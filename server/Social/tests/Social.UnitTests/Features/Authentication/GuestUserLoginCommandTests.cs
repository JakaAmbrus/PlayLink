using Social.Application.Features.Authentication.Common;
using Social.Application.Features.Authentication.GuestUserLogin;
using Social.Application.Interfaces;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;
using Social.UnitTests.Configurations;

namespace Social.UnitTests.Features.Authentication
{
    public class GuestUserLoginCommandTests
    {
        private readonly IMediator _mediator;
        private readonly ISocialDbContext _context;
        private readonly ITokenService _tokenService;

        public GuestUserLoginCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            _tokenService = Substitute.For<ITokenService>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GuestUserLoginCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new GuestUserLoginCommandHandler(_context, _tokenService)
                .Handle(c.Arg<GuestUserLoginCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(ISocialDbContext context)
        {
            context.Users.Add(new User { Id = 1, Username = "testone" });
            context.Users.Add(new User { Id = 2, Username = "testtwo" });
            context.Users.Add(new User { Id = 3, Username = "testthree" });
            context.Users.Add(new User { Id = 4, Username = "modone" });
            context.Users.Add(new User { Id = 5, Username = "modtwo" });
            context.Users.Add(new User { Id = 6, Username = "modthree" });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GuestUserLogin_ShouldReturnMemberUserDTO_WhenGuestUserExists()
        {
            // Arrange
            var memberUsernames = new List<string> { "testone", "testtwo", "testthree" };

            var request = new GuestUserLoginCommand
            {
                 Role = "Member"
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<GuestUserLoginResponse>();
            response.User.Should().BeOfType<UserDto>();
            response.User.Username.Should().BeOneOf(memberUsernames);
        }

        [Fact]
        public async Task GuestUserLogin_ShouldReturnModeratorUserDTO_WhenGuestUserExists()
        {
            // Arrange
            var moderatorUsernames = new List<string> { "modone", "modtwo", "modthree" };

            var request = new GuestUserLoginCommand
            {
                 Role = "Moderator"
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<GuestUserLoginResponse>();
            response.User.Should().BeOfType<UserDto>();
            response.User.Username.Should().BeOneOf(moderatorUsernames);
        }

        [Fact]
        public async Task GuestUserLogin_ShouldThrowNotFoundException_WhenGuestUserDoesNotExist()
        {
            // Arrange
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChangesAsync(CancellationToken.None).Wait();

            var request = new GuestUserLoginCommand
            {
                 Role = "Member"
            };

            // Act
            Func<Task> act = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Guest user not found");
        }
    }
}
