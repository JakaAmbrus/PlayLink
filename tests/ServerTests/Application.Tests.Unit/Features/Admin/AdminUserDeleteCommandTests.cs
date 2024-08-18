using Application.Features.Admin.AdminUserDelete;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Tests.Unit.Features.Admin
{
    public class AdminUserDeleteCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IUserManager _userManager;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public AdminUserDeleteCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            _userManager = Substitute.For<IUserManager>();
            _cacheInvalidationService = Substitute.For<ICacheInvalidationService>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<AdminUserDeleteCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new AdminUserDeleteCommandHandler(_context, _userManager, _cacheInvalidationService)
                .Handle(c.Arg<AdminUserDeleteCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser
            {
                Id = 1,
                UserName = "AdminTestUser",
                UserRoles = new List<AppUserRole>
                {
                    new AppUserRole
                    {
                        Role = new AppRole
                        {
                            Name = "Admin"
                        }
                    }
                },
            });
            context.Users.Add(new AppUser { Id = 2, UserName = "Tester" });

            context.Connections.Add(new Connection { ConnectionId = "1", Username = "Tester" });

            context.Posts.Add(new Post { PostId = 1, AppUserId = 2 });

            context.Likes.Add(new Like { LikeId = 1, PostId = 1, AppUserId = 2 });

            context.FriendRequests.Add(new FriendRequest { FriendRequestId = 1, SenderId = 2, ReceiverId = 1 });

            context.Friendships.Add(new Friendship { FriendshipId = 1, User1Id = 2, User2Id = 1 });

            context.PrivateMessages.Add(new PrivateMessage { PrivateMessageId = 1, SenderId = 2, RecipientId = 1 });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task AdminUserDelete_ShouldDeleteUserAndRelatedEntities_WhenUserExists()
        {
            // Arrange
            _userManager.IsInRoleAsync(Arg.Any<AppUser>(), "Admin").Returns(Task.FromResult(true));

            var request = new AdminUserDeleteCommand
            {
                AppUserId = 2,
                AuthUserId = 1,
                AuthUserRoles = new List<string> { "Admin" }
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().BeOfType<AdminUserDeleteResponse>();
            response.UserDeleted.Should().BeTrue();
            _context.Users.Count().Should().Be(1);
            _context.Connections.Count().Should().Be(0);
            _context.Posts.Count().Should().Be(0);
            _context.Likes.Count().Should().Be(0);
            _context.FriendRequests.Count().Should().Be(0);
            _context.PrivateMessages.Count().Should().Be(0);
            _context.Friendships.Count().Should().Be(0);
        }

        [Fact]
        public async Task AdminUserDelete_ShouldInvalidateCache_WhenUserIsDeleted()
        {
            // Arrange
            _userManager.IsInRoleAsync(Arg.Any<AppUser>(), "Admin").Returns(Task.FromResult(true));

            var request = new AdminUserDeleteCommand
            {
                AppUserId = 2,
                AuthUserId = 1,
                AuthUserRoles = new List<string> { "Admin" }
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().BeOfType<AdminUserDeleteResponse>();
            response.UserDeleted.Should().BeTrue();
            _cacheInvalidationService.Received(1).InvalidateSearchUserCache();
            _cacheInvalidationService.Received(1).InvalidateNearestBirthdayUsersCache();
        }

        [Fact]
        public async Task AdminUserDelete_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new AdminUserDeleteCommand
            {
                AppUserId = 3,
                AuthUserId = 1,
                AuthUserRoles = new List<string> { "Admin" }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User not found");
        }

        [Fact]
        public async Task AdminUserDelete_ShouldThrowNotFoundException_WhenAuthUserDoesNotExist()
        {
            // Arrange
            var request = new AdminUserDeleteCommand
            {
                AppUserId = 2,
                AuthUserId = 3,
                AuthUserRoles = new List<string> { "Admin" }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Authorized user not found");
        }

        [Fact]
        public async Task AdminUserDelete_ShouldThrowUnauthorizedException_WhenUserIsNotAdmin()
        {
            // Arrange
            _userManager.IsInRoleAsync(Arg.Any<AppUser>(), "Admin").Returns(Task.FromResult(false));

            var request = new AdminUserDeleteCommand
            {
                AppUserId = 1,
                AuthUserId = 2,
                AuthUserRoles = new List<string> { "Admin" }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("Unauthorized, only an Admin can delete a user");
        }
    }
}
