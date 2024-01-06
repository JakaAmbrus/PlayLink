using Application.Exceptions;
using Application.Features.Users.DeleteUser;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using MediatR;

namespace Application.Tests.Unit.Features.Users
{
    public class DeleteUserCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public DeleteUserCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            _cacheInvalidationService = Substitute.For<ICacheInvalidationService>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<DeleteUserCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new DeleteUserCommandHandler(_context, _cacheInvalidationService)
                .Handle(c.Arg<DeleteUserCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1, UserName = "tester" });
            context.Users.Add(new AppUser 
            { 
                Id = 2,
                UserName = "tester2",
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
            context.Users.Add(new AppUser
            {
                Id = 3,
                UserName = "tester3",
                UserRoles = new List<AppUserRole>
                {
                    new AppUserRole
                    {
                        Role = new AppRole
                        {
                            Name = "Guest"
                        }
                    }
                },
            });


            context.Connections.Add(new Connection { ConnectionId = "1", Username = "tester" });

            context.Posts.Add(new Post { PostId = 1, AppUserId = 1 });

            context.Likes.Add(new Like { LikeId = 1, PostId = 1, AppUserId = 1 });

            context.FriendRequests.Add(new FriendRequest { FriendRequestId = 1, SenderId = 1, ReceiverId = 2 });
            context.FriendRequests.Add(new FriendRequest { FriendRequestId = 2, SenderId = 2, ReceiverId = 1 });

            context.Friendships.Add(new Friendship { FriendshipId = 1, User1Id = 1, User2Id = 2 });

            context.PrivateMessages.Add(new PrivateMessage
            {
                PrivateMessageId = 2,
                SenderId = 1,
                RecipientId = 2
            });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task DeleteUser_ShouldDeleteUserAndRelatedEntities_WhenUserExists()
        {
            // Arrange
            var request = new DeleteUserCommand
            {
                AuthUserId = 1,
                AuthUserRoles = new List<string> { "Member" }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.IsDeleted.Should().BeTrue();
            _context.Users.Should().HaveCount(2);
            _context.Connections.Should().BeEmpty();
            _context.Posts.Should().BeEmpty();
            _context.Likes.Should().BeEmpty();
            _context.FriendRequests.Should().BeEmpty();
            _context.Friendships.Should().BeEmpty();
            _context.PrivateMessages.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteUser_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new DeleteUserCommand
            {
                AuthUserId = 4,
                AuthUserRoles = new List<string> { "Member" }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Authorized user not found");
        }

        [Fact]
        public async Task DeleteUser_ShouldInvalidateCaches_WhenUserIsDeleted()
        {
            // Arrange
            var request = new DeleteUserCommand
            {
                AuthUserId = 1,
                AuthUserRoles = new List<string> { "Member" }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.IsDeleted.Should().BeTrue();
            _cacheInvalidationService.Received(1).InvalidateSearchUserCache();
            _cacheInvalidationService.Received(1).InvalidateNearestBirthdayUsersCache();
        }

        [Fact]
        public async Task DeleteUser_ShouldThrowUnauthorizedException_WhenUserIsAdmin()
        {
            // Arrange
            var request = new DeleteUserCommand
            {
                AuthUserId = 2,
                AuthUserRoles = new List<string> { "Admin" }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("An Administrator account cannot be deleted");
        }

        [Fact]
        public async Task DeleteUser_ShouldThrowUnauthorizedException_WhenUserIsGuest()
        {
            // Arrange
            var request = new DeleteUserCommand
            {
                AuthUserId = 3,
                AuthUserRoles = new List<string> { "Guest" }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("Cannot delete a Guest user account");
        }
    }
}
