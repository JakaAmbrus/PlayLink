using Application.Features.Users.Common;
using Application.Features.Users.GetNearestBirthdayUsers;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Tests.Unit.Features.Users
{
    public class GetNearestBirthdayUsersQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public GetNearestBirthdayUsersQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            _memoryCache = Substitute.For<IMemoryCache>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetNearestBirthdayUsersQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetNearestBirthdayUsersQueryHandler(_context, _memoryCache)
                .Handle(c.Arg<GetNearestBirthdayUsersQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser
            {
                Id = 1,
                UserName = "tester",
                DateOfBirth = new DateTime(1999, 5, 16)
            });
            context.Users.Add(new AppUser
            {
                Id = 2,
                UserName = "tester2",
                DateOfBirth = new DateTime(2002, 5, 16)
            });
            context.Users.Add(new AppUser
            {
                Id = 3,
                UserName = "tester3",
                DateOfBirth = new DateTime(1990, 5, 16)
            });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetNearestBirthdayUsers_ShouldReturnThreeNearestBirthdayUsers_WhenCalledAndThereIsNoCache()
        {
            // Arrange
            var request = new GetNearestBirthdayUsersQuery { };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Users.Should().NotBeNull();
            response.Users.Should().BeOfType<List<UserBirthdayDto>>();
            response.Users.Count.Should().Be(3);
        }

        [Fact]
        public async Task GetNearestBirthdayUsers_ShouldReturnEmptyList_WhenNoUsersInDatabaseAndThereIsNoCache()
        {
            // Arrange
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync(CancellationToken.None);

            var request = new GetNearestBirthdayUsersQuery { };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Users.Should().NotBeNull();
            response.Users.Should().BeOfType<List<UserBirthdayDto>>();
            response.Users.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetNearestBirthdayUsers_ShouldReturnThreeNearestBirthdayUsers_WhenCalledAndThereIsCache()
        {
            // Arrange
            var cachedBirthdayUsers = Enumerable.Range(1, 3)
                .Select(i => new UserBirthdayDto
                {
                    Username = $"tester{i}",
                    FullName = $"{i} Tester",
                    DateOfBirth = new DateOnly(1999, 5, 16)
                }).ToList();

            _memoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<List<UserBirthdayDto>>())
                .Returns(x =>
                {
                    x[1] = cachedBirthdayUsers;
                    return true;
                });

            var request = new GetNearestBirthdayUsersQuery { };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Users.Should().NotBeNull();
            response.Users.Should().BeEquivalentTo(cachedBirthdayUsers);
        }

        [Fact]
        public async Task GetNearestBirthdayUsers_ShouldReturnEmptyList_WhenNoUsersInDatabaseAndThereIsCache()
        {
            // Arrange
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync(CancellationToken.None);

            var emptyBirthdayUsers = new List<UserBirthdayDto>();

            _memoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<List<UserBirthdayDto>>())
                .Returns(x =>
                {
                    x[1] = emptyBirthdayUsers;
                    return true;
                });

            var request = new GetNearestBirthdayUsersQuery { };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Users.Should().NotBeNull();
            response.Users.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetNearestBirthdayUsers_ShouldReturnThreeNearestBirthdayUsers_WhenThereAreMoreUsersInDatabase()
        {
            // Arrange
            var users = Enumerable.Range(4, 10)
                .Select(i => new AppUser
                {
                    Id = i,
                    UserName = $"tester{i}",
                    DateOfBirth = new DateTime(1999, 5, 16)
                }).ToList();
            _context.Users.AddRange(users);

            await _context.SaveChangesAsync(CancellationToken.None);

            var request = new GetNearestBirthdayUsersQuery { };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Users.Should().NotBeNull();
            response.Users.Should().BeOfType<List<UserBirthdayDto>>();
            response.Users.Count.Should().Be(3);
        }
    }
}
