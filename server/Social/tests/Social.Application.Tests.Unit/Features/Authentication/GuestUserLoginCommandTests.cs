using Social.Application.Features.Authentication.Common;
using Social.Application.Features.Authentication.GuestUserLogin;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;

namespace Social.Application.Tests.Unit.Features.Authentication
{
    public class GuestUserLoginCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
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

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1, UserName = "testone" });
            context.Users.Add(new AppUser { Id = 2, UserName = "testtwo" });
            context.Users.Add(new AppUser { Id = 3, UserName = "testthree" });
            context.Users.Add(new AppUser { Id = 4, UserName = "modone" });
            context.Users.Add(new AppUser { Id = 5, UserName = "modtwo" });
            context.Users.Add(new AppUser { Id = 6, UserName = "modthree" });

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
