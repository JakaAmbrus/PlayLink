using Social.Application.Features.Users.Common;
using Social.Application.Features.Users.GetUsersForSearchBar;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Social.Application.Tests.Unit.Features.Users
{
    public class GetUsersForSearchBarQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public GetUsersForSearchBarQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            _memoryCache = Substitute.For<IMemoryCache>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetUsersForSearchBarQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetUsersForSearchBarQueryHandler(_context, _memoryCache)
                .Handle(c.Arg<GetUsersForSearchBarQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            var users = Enumerable.Range(1, 10).Select(i => new AppUser
            {
                Id = i,
                UserName = $"tester{i}",
                FullName = $"tester {i}",
            }).ToList();
            context.Users.AddRange(users);

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetUsersForSearchBar_ShouldReturnAListOfUserDtos_WhenCalledAndThereIsNoCache()
        {
            // Arrange
            var request = new GetUsersForSearchBarQuery { };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Users.Should().NotBeNull();
            response.Users.Should().BeOfType<List<SearchUserDto>>();
            response.Users.Count.Should().Be(10);
        }

        [Fact]
        public async Task GetUsersForSearchBar_ShouldReturnAListOfUserDtos_WhenCalledAndThereIsCache()
        {
            // Arrange
            var cachedSearchUsers = Enumerable.Range(1, 10).Select(i => new SearchUserDto
            {
                AppUserId = i,
                Username = $"tester{i}",
                FullName = $"tester {i}",
            }).ToList();

            _memoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<List<SearchUserDto>>())
                .Returns(x =>
                {
                    x[1] = cachedSearchUsers;
                    return true;
                });

            var request = new GetUsersForSearchBarQuery { };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Users.Should().NotBeNull();
            response.Users.Count.Should().Be(10);
            response.Users.Should().BeEquivalentTo(cachedSearchUsers);
        }

        [Fact]
        public async Task GetUsersForSearchBar_ShouldReturnEmptyList_WhenThereAreNoUsersInDatabaseAndThereIsNoCache()
        {
            // Arrange
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync(CancellationToken.None);

            var request = new GetUsersForSearchBarQuery { };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Users.Should().NotBeNull();
            response.Users.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetUsersForSearchBar_ShouldReturnEmptyList_WhenThereAreNoUsersInDatabaseAndThereIsCache()
        {
            // Arrange
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync(CancellationToken.None);

            var emptySearchUsers = new List<SearchUserDto>();

            _memoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<List<SearchUserDto>>())
                .Returns(x =>
                {
                    x[1] = emptySearchUsers;
                    return true;
                });

            var request = new GetUsersForSearchBarQuery { };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Users.Should().NotBeNull();
            response.Users.Count.Should().Be(0);
        }
    }
}
