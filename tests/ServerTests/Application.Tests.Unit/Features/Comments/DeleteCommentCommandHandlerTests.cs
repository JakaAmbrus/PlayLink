using Application.Exceptions;
using Application.Features.Comments.DeleteComment;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;

namespace Application.Tests.Unit.Features.Comments
{
    public class DeleteCommentCommandHandlerTests
    {
        private readonly DeleteCommentCommandHandler _handler;
        private readonly IApplicationDbContext _context;

        public DeleteCommentCommandHandlerTests()
        {
            _context = TestBase.CreateTestDbContext();
            _handler = new DeleteCommentCommandHandler(_context);

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1 });
            context.Posts.Add(new Post { PostId = 1, CommentsCount = 1 });
            context.Comments.Add(new Comment { CommentId = 1, PostId = 1, AppUserId = 1 });
            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task Handle_ShouldDeleteCommentAndReturnTheCorrectResponse_WhenUserIsTheOwner()
        {
            // Arrange
            var request = new DeleteCommentCommand { CommentId = 1, AuthUserId = 1, AuthUserRoles = new List<string>() };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsDeleted.Should().BeTrue();
            _context.Comments.Count().Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldDeleteCommentAndReturnTheCorrectResponse_WhenUserIsModerator()
        {
            // Arrange
            var request = new DeleteCommentCommand { CommentId = 1, AuthUserId = 1, AuthUserRoles = new List<string> { "Moderator" } };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsDeleted.Should().BeTrue();
            _context.Comments.Count().Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldDecrementCommentsCount_WhenCommentIsDeleted()
        {
            // Arrange
            var request = new DeleteCommentCommand { CommentId = 1, AuthUserId = 1, AuthUserRoles = new List<string>() };

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            var updatedPost = await _context.Posts.FindAsync(1);
            updatedPost.CommentsCount.Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedException_WhenUserIsNotTheOwnerAndNotModerator()
        {
            // Arrange
            _context.Users.Add(new AppUser { Id = 2 });
            _context.SaveChangesAsync(CancellationToken.None).Wait();

            var request = new DeleteCommentCommand { CommentId = 1, AuthUserId = 2, AuthUserRoles = new List<string>() };

            // Act
            var result = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await result.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("User not authorized to delete comment");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCommentIsNotFound()
        {
            // Arrange
            var request = new DeleteCommentCommand { CommentId = 0, AuthUserId = 1, AuthUserRoles = new List<string>() };

            // Act
            var result = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Comment was not found");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenPostIsNotFound()
        {
            // Arrange
            _context.Comments.Add(new Comment { CommentId = 2, PostId = 0, AppUserId = 1 });
            _context.SaveChangesAsync(CancellationToken.None).Wait();

            var request = new DeleteCommentCommand { CommentId = 2, AuthUserId = 1, AuthUserRoles = new List<string>() };

            // Act
            var result = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Post was not found");
        }
    }
}
