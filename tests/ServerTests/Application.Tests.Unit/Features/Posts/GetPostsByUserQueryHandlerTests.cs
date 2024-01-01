using Application.Exceptions;
using Application.Features.Posts.Common;
using Application.Features.Posts.GetPostsByUser;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Application.Utils;
using Domain.Entities;

namespace Application.Tests.Unit.Features.Posts
{
    public class GetPostsByUserQueryHandlerTests
    {
        private readonly GetPostsByUserQueryHandler _handler;
        private readonly IApplicationDbContext _context;

        public GetPostsByUserQueryHandlerTests()
        {
            _context = TestBase.CreateTestDbContext();
            _handler = new GetPostsByUserQueryHandler(_context);

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1, UserName = "Tester1" });
            context.Users.Add(new AppUser { Id = 2, UserName = "Tester2" });
            var posts = Enumerable.Range(1, 10)
                             .Select(i => new Post
                             {
                                 PostId = i,
                                 AppUserId = 1,
                                 DatePosted = DateTime.UtcNow.AddMinutes(-i),
                                 CommentsCount = i
                             }).ToList();

            context.Posts.AddRange(posts);
            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedListOfPostDTOs_WhenUserHasPosts()
        {
            // Arrange
            var request = new GetPostsByUserQuery
            {
                Username = "Tester1",
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 },
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Posts.Should().NotBeNull();
            response.Posts.Count().Should().Be(5);
            response.Posts.First().Should().BeOfType<PostDto>();
        }

        [Fact]
        public async Task Handle_ShouldReturnAnEmptyPagedList_WhenUserHasNoPosts()
        {
            // Arrange
            var request = new GetPostsByUserQuery
            {
                Username = "Tester2",
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 },
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Posts.Should().NotBeNull();
            response.Posts.Count().Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedListOfPostDTOs_WhenUserHasLessThanPageSizePosts()
        {
            // Arrange
            var request = new GetPostsByUserQuery
            {
                Username = "Tester1",
                Params = new PaginationParams { PageNumber = 1, PageSize = 15 },
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Posts.Should().NotBeNull();
            response.Posts.Count().Should().Be(10);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new GetPostsByUserQuery
            {
                Username = "NonExistentUser",
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 },
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            var result = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User with Username NonExistentUser not found");
        }
    }
}
