using Application.Exceptions;
using Application.Features.Admin.AdminEditRoles;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using MediatR;

namespace Application.Tests.Unit.Features.Admin
{
    public class AdminEditRolesCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IUserManager _userManager;

        public AdminEditRolesCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            _userManager = Substitute.For<IUserManager>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<AdminEditRolesCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new AdminEditRolesCommandHandler(_context, _userManager)
                .Handle(c.Arg<AdminEditRolesCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1, UserName = "FakeTestUser" });
            context.Users.Add(new AppUser { Id = 2, UserName = "FakeTestUser2" });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task AdminEditRoles_ShouldAddModeratorRoleAndReturnTrue_WhenUserIsNotModerator()
        {
            // Arrange
            var request = new AdminEditRolesCommand
            {
                AppUserId = 2,
                AuthUserId = 1
            };

            _userManager.IsInRoleAsync(Arg.Any<AppUser>(), "Admin").Returns(Task.FromResult(true));
            _userManager.IsInRoleAsync(Arg.Any<AppUser>(), "Moderator").Returns(Task.FromResult(false));

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().BeOfType<AdminEditRolesResponse>();
            response.RoleEdited.Should().BeTrue();
            await _userManager.Received(1).AddToRoleAsync(Arg.Is<AppUser>(u => u.Id == request.AppUserId), "Moderator");
        }

        [Fact]
        public async Task AdminEditRoles_ShouldRemoveModeratorRoleAndReturnTrue_WhenUserIsModerator()
        {
            // Arrange
            var request = new AdminEditRolesCommand
            {
                AppUserId = 2,
                AuthUserId = 1
            };

            _userManager.IsInRoleAsync(Arg.Any<AppUser>(), "Admin").Returns(Task.FromResult(true));
            _userManager.IsInRoleAsync(Arg.Any<AppUser>(), "Moderator").Returns(Task.FromResult(true));

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().BeOfType<AdminEditRolesResponse>();
            response.RoleEdited.Should().BeTrue();
            await _userManager.Received(1).RemoveFromRoleAsync(Arg.Is<AppUser>(u => u.Id == request.AppUserId), "Moderator");
        }

        [Fact]
        public async Task AdminEditRoles_ShouldThrowNotFoundException_WhenUserToEditDoesNotExist()
        {
            // Arrange
            var request = new AdminEditRolesCommand
            {
                AppUserId = 3,
                AuthUserId = 1
            };

            // Act
            var action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User to edit not found");
        }

        [Fact]
        public async Task AdminEditRoles_ShouldThrowNotFoundException_WhenAuthUserDoesNotExist()
        {
            // Arrange
            var request = new AdminEditRolesCommand
            {
                AppUserId = 2,
                AuthUserId = 3
            };

            // Act
            var action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Auth user not found");
        }

        [Fact]
        public async Task AdminEditRoles_ShouldThrowUnauthorizedAccessException_WhenAuthUserIsNotAdmin()
        {
            // Arrange
            var request = new AdminEditRolesCommand
            {
                AppUserId = 2,
                AuthUserId = 1
            };

            _userManager.IsInRoleAsync(Arg.Any<AppUser>(), "Admin").Returns(Task.FromResult(false));

            // Act
            var action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Unauthorized, only an Admin can make this request");
        }
    }
}
