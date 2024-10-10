using Social.Application.Features.Comments.Common;
using Social.Application.Features.Comments.GetComments;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Application.Utils;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;
using Social.Application.Features.Comments.GetPostComments;

namespace Social.Application.Tests.Unit.Features.Comments
{
    public class GetPostCommentsQueryHandlerTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public GetPostCommentsQueryHandlerTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetPostCommentsQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetPostCommentsQueryHandler(_context)
                .Handle(c.Arg<GetPostCommentsQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1 });
            context.Posts.Add(new Post { PostId = 1, CommentsCount = 2 });
            var comments = Enumerable.Range(1, 10)
                .Select(i => new Comment
                {
                    AppUserId = 1,
                    CommentId = i,
                    PostId = 1,
                    Content = $"Comment {i}",
                    TimeCommented = DateTime.UtcNow.AddMinutes(-i)
                }).ToList();

            context.Comments.AddRange(comments);
            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnPagedListOfCommentDTOs_WhenPostHasComments()
        {
            // Arrange
            var request = new GetPostCommentsQuery 
            { 
                PostId = 1,
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 },
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Comments.Should().NotBeNull();
            response.Comments.Count().Should().Be(5);
            response.Comments.Should().AllBeOfType<CommentDto>();
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnAndEmptyList_WhenPostHasNoComments()
        {
            // Arrange
            _context.Posts.Add(new Post { PostId = 2, CommentsCount = 0 });
            _context.SaveChangesAsync(CancellationToken.None).Wait();

            var request = new GetPostCommentsQuery 
            { 
                PostId = 2,
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 }     ,
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Comments.Should().NotBeNull();
            response.Comments.Count().Should().Be(0);
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnAnEmptyList_WhenPageNumberIsGreaterThanTotalPages()
        {
            // Arrange
            var request = new GetPostCommentsQuery
            {
                PostId = 1,
                Params = new PaginationParams { PageNumber = 3, PageSize = 5 },
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Comments.Should().NotBeNull();
            response.Comments.Count().Should().Be(0);
        }

        [Fact]
        public async Task ReturnPagedListOfCommentDTOs_WhenPostHasLessCommentsThanPageSize()
        {
            // Arrange
            var request = new GetPostCommentsQuery
            {
                PostId = 1,
                Params = new PaginationParams { PageNumber = 1, PageSize = 20 },
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Comments.Should().NotBeNull();
            response.Comments.Count().Should().Be(10);
            response.Comments.Should().AllBeOfType<CommentDto>();
        }
        [Fact]
        public async Task GetPostComments_ShouldReturnIsAuthorizedTrue_WhenUserIsTheOwner()
        {
            // Arrange
            var request = new GetPostCommentsQuery
            {
                PostId = 1,
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 },
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Comments[0].IsAuthorized.Should().BeTrue();
        }

        [Fact]
        public async Task GetPostComments_ShouldReturnIsAuthorizedTrue_WhenUserIsModerator()
        {
            // Arrange
            _context.Users.Add(new AppUser { Id = 2 });
            _context.SaveChangesAsync(CancellationToken.None).Wait();

            var request = new GetPostCommentsQuery
            {
                PostId = 1,
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 },
                AuthUserId = 2,
                AuthUserRoles = new List<string> { "Moderator" }
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Comments[0].IsAuthorized.Should().BeTrue();
        }

        [Fact]
        public async Task GetPostComments_ShouldThrowNotFoundException_WhenPostDoesNotExist()
        {
            // Arrange
            var request = new GetPostCommentsQuery
            {
                PostId = 3,
                Params = new PaginationParams { PageNumber = 1, PageSize = 5 },
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Post not found");
        }
    }
}
