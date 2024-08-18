using Application.Features.Comments.DeleteComment;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Tests.Unit.Features.Comments
{
    public class DeleteCommentCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public DeleteCommentCommandTests()
        {
            _context = TestBase.CreateTestDbContext();           
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<DeleteCommentCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new DeleteCommentCommandHandler(_context)
                .Handle(c.Arg<DeleteCommentCommand>(), c.Arg<CancellationToken>()));

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
        public async Task DeleteComment_ShouldDeleteCommentAndReturnTheCorrectResponse_WhenUserIsTheOwner()
        {
            // Arrange
            var request = new DeleteCommentCommand { CommentId = 1, AuthUserId = 1, AuthUserRoles = new List<string>() };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsDeleted.Should().BeTrue();
            _context.Comments.Count().Should().Be(0);
        }

        [Fact]
        public async Task DeleteComment_ShouldDeleteCommentAndReturnTheCorrectResponse_WhenUserIsModerator()
        {
            // Arrange
            var request = new DeleteCommentCommand { CommentId = 1, AuthUserId = 1, AuthUserRoles = new List<string> { "Moderator" } };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsDeleted.Should().BeTrue();
            _context.Comments.Count().Should().Be(0);
        }

        [Fact]
        public async Task DeleteComment_ShouldDecrementCommentsCount_WhenCommentIsDeleted()
        {
            // Arrange
            var request = new DeleteCommentCommand { CommentId = 1, AuthUserId = 1, AuthUserRoles = new List<string>() };

            // Act
            await _mediator.Send(request, CancellationToken.None);

            // Assert
            var updatedPost = await _context.Posts.FindAsync(1);
            updatedPost.CommentsCount.Should().Be(0);
        }

        [Fact]
        public async Task DeleteComment_ShouldThrowUnauthorizedException_WhenUserIsNotTheOwnerAndNotModerator()
        {
            // Arrange
            _context.Users.Add(new AppUser { Id = 2 });
            _context.SaveChangesAsync(CancellationToken.None).Wait();

            var request = new DeleteCommentCommand { CommentId = 1, AuthUserId = 2, AuthUserRoles = new List<string>() };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("User not authorized to delete comment");
        }

        [Fact]
        public async Task DeleteComment_ShouldThrowNotFoundException_WhenCommentIsNotFound()
        {
            // Arrange
            var request = new DeleteCommentCommand { CommentId = 0, AuthUserId = 1, AuthUserRoles = new List<string>() };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Comment was not found");
        }

        [Fact]
        public async Task DeleteComment_ShouldThrowNotFoundException_WhenPostIsNotFound()
        {
            // Arrange
            _context.Comments.Add(new Comment { CommentId = 2, PostId = 0, AppUserId = 1 });
            _context.SaveChangesAsync(CancellationToken.None).Wait();

            var request = new DeleteCommentCommand { CommentId = 2, AuthUserId = 1, AuthUserRoles = new List<string>() };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Post was not found");
        }
    }
}
