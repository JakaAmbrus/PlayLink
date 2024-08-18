using Application.Features.Users.Common;
using Application.Features.Users.GetUserByUsername;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Tests.Unit.Features.Users
{
    public class GetUserByUsernameQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public GetUserByUsernameQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            _memoryCache = Substitute.For<IMemoryCache>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetUserByUsernameQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetUserByUsernameQueryHandler(_context, _memoryCache)
                .Handle(c.Arg<GetUserByUsernameQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser
            {
                Id = 1,
                UserName = "tester",
                FullName = "Tester Test",
                Gender = "male",
                Country = "USA",
                ProfilePictureUrl = "https://res.cloudinary.com",
                DateOfBirth = new DateOnly(1999, 5, 16),
                Description = "test description",
                LastActive = DateTime.UtcNow,
                Created = DateTime.UtcNow - TimeSpan.FromDays(1),
            });
            context.Users.Add(new AppUser
            {
                Id = 2,
                UserName = "tester2",
            });
            context.Users.Add(new AppUser
            {
                Id = 3,
                UserName = "tester3",
            });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetUserByUsername_ShouldReturnProfileUserDtoWithAuthorizedFalse_WhenUserIsCurrentUser()
        {
            // Arrange
            var request = new GetUserByUsernameQuery 
            { 
                Username = "tester",
                AuthUserId = 1,
                AuthUserRoles = new List<string> { "Member" }
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.User.Should().NotBeNull();
            response.User.Should().BeOfType<ProfileUserDto>();
            response.User.Authorized.Should().BeFalse();
            response.User.Username.Should().Be("tester");
            response.User.FullName.Should().Be("Tester Test");
            response.User.LastActive.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(5000));
            response.User.Created.Should().BeCloseTo(DateTime.UtcNow - TimeSpan.FromDays(1), TimeSpan.FromMilliseconds(5000));
            response.User.Country.Should().Be("USA");
            response.User.Gender.Should().Be("male");
            response.User.Description.Should().Be("test description");
        }

        [Fact]
        public async Task GetUserByUsername_ShouldReturnProfileUserDtoWithAuthorizedFalse_WhenUserIsNotCurrentUser()
        {
            // Arrange
            var request = new GetUserByUsernameQuery
            {
                Username = "tester",
                AuthUserId = 2,
                AuthUserRoles = new List<string> { "Member" }
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.User.Should().NotBeNull();
            response.User.Should().BeOfType<ProfileUserDto>();
            response.User.Authorized.Should().BeFalse();
            response.User.Username.Should().Be("tester");
            response.User.FullName.Should().Be("Tester Test");
            response.User.LastActive.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(5000));
            response.User.Created.Should().BeCloseTo(DateTime.UtcNow - TimeSpan.FromDays(1), TimeSpan.FromMilliseconds(5000));
            response.User.Country.Should().Be("USA");
            response.User.Gender.Should().Be("male");
            response.User.Description.Should().Be("test description");
        }
        [Fact]
        public async Task GetUserByUsername_ShouldReturnProfileUserDtoWithAuthorizedTrue_WhenUserIsModeratorAndNotCurrentUser()
        {
            // Arrange
            var request = new GetUserByUsernameQuery
            {
                Username = "tester",
                AuthUserId = 2,
                AuthUserRoles = new List<string> { "Moderator" }
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.User.Should().NotBeNull();
            response.User.Should().BeOfType<ProfileUserDto>();
            response.User.Authorized.Should().BeTrue();
            response.User.Username.Should().Be("tester");
            response.User.FullName.Should().Be("Tester Test");
            response.User.LastActive.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(5000));
            response.User.Created.Should().BeCloseTo(DateTime.UtcNow - TimeSpan.FromDays(1), TimeSpan.FromMilliseconds(5000));
            response.User.Country.Should().Be("USA");
            response.User.Gender.Should().Be("male");
            response.User.Description.Should().Be("test description");
        }

        [Fact]
        public async Task GetUserByUsername_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new GetUserByUsernameQuery
            {
                Username = "tester4",
                AuthUserId = 2,
                AuthUserRoles = new List<string> { "Moderator" }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("The user by the username: tester4 not found ");
        }
    }
}
