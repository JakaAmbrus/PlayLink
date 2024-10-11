using Social.Application.Features.Posts.Common;
using Social.Application.Features.Posts.GetPosts;
using Social.Application.Interfaces;
using Social.Application.Utils;
using Social.Domain.Entities;
using MediatR;
using Social.UnitTests.Configurations;

namespace Social.UnitTests.Features.Posts
{
    public class GetPostsQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public GetPostsQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetPostsQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetPostsQueryHandler(_context)
                .Handle(c.Arg<GetPostsQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1 });

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
        public async Task GetPosts_ShouldReturnPagedListOfPostDTOs_WhenPostsExist()
        {
            // Arrange
            var request = new GetPostsQuery
            {
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 },
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Posts.Should().NotBeNull();
            response.Posts.Should().AllBeOfType<PostDto>();
            response.Posts.Count.Should().Be(5);
        }

        [Fact]
        public async Task GetPosts_ShouldReturnAnEmptyList_WhenThereAreNoPosts()
        {
            // Arrange
            var request = new GetPostsQuery
            {
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 },
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            _context.Posts.RemoveRange(_context.Posts);
            await _context.SaveChangesAsync(CancellationToken.None);

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Posts.Should().NotBeNull();
            response.Posts.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetPosts_ShouldReturnAnEmptyList_WhenPageNumberIsGreaterThanTotalPages()
        {
            // Arrange
            var request = new GetPostsQuery
            {
                Params = new PaginationParams { PageNumber = 3, PageSize = 10 },
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Posts.Should().NotBeNull();
            response.Posts.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetPosts_ShouldReturnPagedListOfPostDTOs_WhenThereAreLessPostsThanPageSize()
        {
            // Arrange
            var request = new GetPostsQuery
            {
                Params = new PaginationParams { PageNumber = 1, PageSize = 15 },
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Posts.Should().NotBeNull();
            response.Posts.Should().AllBeOfType<PostDto>();
            response.Posts.Count.Should().Be(10);
        }
    }
}
