using Application.Features.Admin.AdminGetUsers;
using Application.Features.Admin.Common;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Application.Utils;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Tests.Unit.Features.Admin
{
    public class AdminGetUsersQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IUserManager _userManager;

        public AdminGetUsersQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            _userManager = Substitute.For<IUserManager>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<AdminGetUsersQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new AdminGetUsersQueryHandler(_context, _userManager)
                .Handle(c.Arg<AdminGetUsersQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 50, UserName = "AdminTestUser" });
            var users = Enumerable.Range(1, 15)
                .Select(i => new AppUser
                {
                    Id = i,
                    UserName = $"{i}"
                }).ToList();
            context.Users.AddRange(users);

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task AdminGetUsers_ShouldReturnPagedListOfUserDTOsWithoutAdmin_WhenUsersExist()
        {
            // Arrange
            _userManager.IsInRoleAsync(Arg.Any<AppUser>(), "Admin").Returns(Task.FromResult(true));

            var request = new AdminGetUsersQuery
            {
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 },
                AuthUserId = 50
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Users.Should().AllBeOfType<UserWithRolesDto>();
            response.Users.Count.Should().Be(5);
            response.Users.Should().NotContain(u => u.AppUserId == 50);
        }

        [Fact]
        public async Task AdminGetUsers_ShouldReturnPagedListOfUserDTOs_WhenPageSizeIsGreaterThanNumberOfUsers()
        {
            // Arrange
            _userManager.IsInRoleAsync(Arg.Any<AppUser>(), "Admin").Returns(Task.FromResult(true));

            var request = new AdminGetUsersQuery
            {
                Params = new PaginationParams { PageNumber = 1, PageSize = 20 },
                AuthUserId = 50
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Users.Should().AllBeOfType<UserWithRolesDto>();
            response.Users.Count.Should().Be(15);
        }

        [Fact]
        public async Task AdminGetUsers_ShouldReturnEmptyList_WhenPageNumberIsGreaterThanNumberOfPages()
        {
            // Arrange
            _userManager.IsInRoleAsync(Arg.Any<AppUser>(), "Admin").Returns(Task.FromResult(true));

            var request = new AdminGetUsersQuery
            {
                Params = new PaginationParams { PageNumber = 4, PageSize = 5 },
                AuthUserId = 50
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Users.Should().AllBeOfType<UserWithRolesDto>();
            response.Users.Count.Should().Be(0);
        }

        [Fact]
        public async Task AdminGetUsers_ShouldReturnAnEmptyList_WhenThereAreNoUsersOtherThanAdmin()
        {
            // Arrange
            _context.Users.RemoveRange(_context.Users);
            _context.Users.Add(new AppUser { Id = 50, UserName = "AdminTestUser" });
            _context.SaveChangesAsync(CancellationToken.None).Wait();

            _userManager.IsInRoleAsync(Arg.Any<AppUser>(), "Admin").Returns(Task.FromResult(true));

            var request = new AdminGetUsersQuery
            {
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 },
                AuthUserId = 50
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Users.Should().BeEmpty();
        }

        [Fact]
        public async Task AdminGetUsers_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            _userManager.IsInRoleAsync(Arg.Any<AppUser>(), "Admin").Returns(Task.FromResult(true));

            var request = new AdminGetUsersQuery
            {
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 },
                AuthUserId = 56
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Authorized user not found");
        }

        [Fact]
        public async Task AdminGetUsers_ShouldThrowUnauthorizedAccessException_WhenUserIsNotAdmin()
        {
            // Arrange
            _userManager.IsInRoleAsync(Arg.Any<AppUser>(), "Admin").Returns(Task.FromResult(false));

            var request = new AdminGetUsersQuery
            {
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 },
                AuthUserId = 50
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("Unauthorized, only an Admin can make this request");
        }
    }
}
