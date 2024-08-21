using Social.Application.Features.Posts.Common;
using Social.Application.Features.Posts.GetPostsByUser;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Application.Utils;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;

namespace Social.Application.Tests.Unit.Features.Posts
{
    public class GetPostsByUserQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public GetPostsByUserQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetPostsByUserQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetPostsByUserQueryHandler(_context)
                .Handle(c.Arg<GetPostsByUserQuery>(), c.Arg<CancellationToken>()));

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
        public async Task GetPostsByUser_ShouldReturnPagedListOfPostDTOs_WhenUserHasPosts()
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
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Posts.Should().NotBeNull();
            response.Posts.Count().Should().Be(5);
            response.Posts.Should().AllBeOfType<PostDto>();
        }

        [Fact]
        public async Task GetPostsByUser_ShouldReturnAnEmptyPagedList_WhenUserHasNoPosts()
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
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Posts.Should().NotBeNull();
            response.Posts.Count().Should().Be(0);
        }

        [Fact]
        public async Task GetPostsByUser_ShouldReturnPagedListOfPostDTOs_WhenUserHasLessThanPageSizePosts()
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
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Posts.Should().NotBeNull();
            response.Posts.Count().Should().Be(10);
        }

        [Fact]
        public async Task GetPostsByUser_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new GetPostsByUserQuery
            {
                Username = "ImaginaryUser",
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 },
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User with Username ImaginaryUser not found");
        }
    }
}
