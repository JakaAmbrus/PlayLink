using Application.Features.Comments.Common;
using Application.Features.Comments.GetComments;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Application.Utils;
using Domain.Entities;

namespace Application.Tests.Unit.Features.Comments
{
    public class GetPostCommentsQueryHandlerTests
    {
        private readonly GetPostCommentsQueryHandler _handler;
        private readonly IApplicationDbContext _context;

        public GetPostCommentsQueryHandlerTests()
        {
            _context = TestBase.CreateTestDbContext();
            _handler = new GetPostCommentsQueryHandler(_context);

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
        public async Task Handle_ShouldReturnPagedListOfCommentDTOs_WhenPostHasComments()
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
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Comments.Should().NotBeNull();
            response.Comments.Count().Should().Be(5);
            response.Comments.First().Should().BeOfType<CommentDto>();
        }

        [Fact]
        public async Task Handle_ShouldReturnTheCorrectResponse_WhenPostHasNoComments()
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
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Comments.Should().NotBeNull();
            response.Comments.Count().Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedListOfCommentDTOs_WhenPostHasLessCommentsThanPageSize()
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
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Comments.Should().NotBeNull();
            response.Comments.Count().Should().Be(10);
            response.Comments.First().Should().BeOfType<CommentDto>();
        }
    }
}
