using Social.Application.Features.Likes.LikeComment;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;

namespace Social.Application.Tests.Unit.Features.Likes
{
    public class LikeCommentCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public LikeCommentCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<LikeCommentCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new LikeCommentCommandHandler(_context)
                .Handle(c.Arg<LikeCommentCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1 });

            context.Posts.Add(new Post { PostId = 1, CommentsCount = 1 });

            context.Comments.Add(new Comment { CommentId = 1, PostId = 1, AppUserId = 1, LikesCount = 1 });
            context.Comments.Add(new Comment { CommentId = 2, PostId = 1, AppUserId = 2, LikesCount = 0 });

            context.Likes.Add(new Like { AppUserId = 1, CommentId = 1 });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task LikeComment_ShouldAddLikeToComment_WhenCommentExists()
        {
            // Arrange
            var request = new LikeCommentCommand { CommentId = 2, AuthUserId = 1 };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().BeOfType<LikeCommentResponse>();
            _context.Comments.Find(2).LikesCount.Should().Be(1);
        }

        [Fact]
        public async Task LikeComment_ShouldReturnLikedTrue_WhenLikeIsAdd()
        {
            // Arrange
            var request = new LikeCommentCommand { CommentId = 2, AuthUserId = 1 };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Liked.Should().BeTrue();
        }

        [Fact]
        public async Task LikeComment_ShouldThrowNotFoundException_WhenCommentDoesNotExist()
        {
            // Arrange
            var request = new LikeCommentCommand { CommentId = 3, AuthUserId = 1 };

            // Act
            Func<Task> act = () => _mediator.Send(request);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Comment not found");
        }

        [Fact]
        public async Task LikeComment_ShouldThrowBadRequestException_WhenCommentIsAlreadyLikedByUser()
        {
            // Arrange
            var request = new LikeCommentCommand { CommentId = 1, AuthUserId = 1 };

            // Act
            Func<Task> act = () => _mediator.Send(request);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("You have already liked this comment");
        }
    }
}
