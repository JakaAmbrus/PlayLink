using Application.Features.Likes.UnlikeComment;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Tests.Unit.Features.Likes
{
    public class UnlikeCommentCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public UnlikeCommentCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<UnlikeCommentCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new UnlikeCommentCommandHandler(_context)
                .Handle(c.Arg<UnlikeCommentCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1 });

            context.Posts.Add(new Post { PostId = 1 });

            context.Comments.Add(new Comment { CommentId = 1, PostId = 1, LikesCount = 1 });
            context.Comments.Add(new Comment { CommentId = 2, PostId = 1, LikesCount = 0 });

            context.Likes.Add(new Like { CommentId = 1, AppUserId = 1 });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task UnlikeComment_ShouldRemoveLikeFromComment_WhenCommentExistsAndIsLikedByUser()
        {
            // Arrange
            var request = new UnlikeCommentCommand { CommentId = 1, AuthUserId = 1 };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            _context.Likes.Count().Should().Be(0);
        }

        [Fact]
        public async Task UnlikeComment_ShouldDecrementLikesCount_WhenCommentExistsAndIsLikedByUser()
        {
            // Arrange
            var request = new UnlikeCommentCommand { CommentId = 1, AuthUserId = 1 };

            // Act
            await _mediator.Send(request);

            // Assert
            _context.Comments.Find(1).LikesCount.Should().Be(0);
        }

        [Fact]
        public async Task UnlikeComment_ShouldReturnUnlikedTrue_WhenLikeIsRemoved()
        {
            // Arrange
            var request = new UnlikeCommentCommand { CommentId = 1, AuthUserId = 1 };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().BeOfType<UnlikeCommentResponse>();
            response.Unliked.Should().BeTrue();
        }

        [Fact]
        public async Task UnlikeComment_ShouldThrowNotFoundException_WhenCommentDoesNotExist()
        {
            // Arrange
            var request = new UnlikeCommentCommand { CommentId = 3, AuthUserId = 1 };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Comment not found");
        }

        [Fact]
        public async Task UnlikeComment_ShouldThrowNotFoundException_WhenLikeDoesNotExist()
        {
            // Arrange
            var request = new UnlikeCommentCommand { CommentId = 2, AuthUserId = 1 };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Comments like not found");
        }
    }
}
