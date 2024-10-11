using Social.Application.Features.Comments.Common;
using Social.Application.Features.Comments.UploadComment;
using Social.Application.Interfaces;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;
using Social.UnitTests.Configurations;

namespace Social.UnitTests.Features.Comments
{
    public class UploadCommentCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public UploadCommentCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<UploadCommentCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new UploadCommentCommandHandler(_context)
                .Handle(c.Arg<UploadCommentCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Posts.Add(new Post { PostId = 1, CommentsCount = 0 });
            context.Users.Add(new AppUser { Id = 1 });
            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task UploadComment_ShouldReturnCorrectCommentResponse_WhenCommentIsUploaded()
        {
            // Arrange
            var comment = new CommentUploadDto { Content = "Test comment content", PostId = 1 };
            var request = new UploadCommentCommand { Comment = comment, AuthUserId = 1 };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.CommentDto.Should().NotBeNull();
            response.CommentDto.Should().BeOfType<CommentDto>();
            response.CommentDto.PostId.Should().Be(comment.PostId);
            response.CommentDto.AppUserId.Should().Be(request.AuthUserId);
            response.CommentDto.LikesCount.Should().Be(0);
            response.CommentDto.IsLikedByCurrentUser.Should().BeFalse();
            response.CommentDto.IsAuthorized.Should().BeTrue();
            response.CommentDto.Content.Should().Be(comment.Content);
        }

        [Fact]
        public async Task UploadComment_ShouldIncrementCommentsCount_WhenCommentIsUploaded()
        {
            // Arrange
            var comment = new CommentUploadDto { Content = "Test comment content", PostId = 1 };
            var request = new UploadCommentCommand { Comment = comment, AuthUserId = 1 };

            // Act
            await _mediator.Send(request, CancellationToken.None);

            // Assert
            var updatedPost = await _context.Posts.FindAsync(1);
            updatedPost.CommentsCount.Should().Be(1);
        }

        [Fact]
        public async Task UploadComment_ShouldThrowNotFoundException_WhenPostIsNotFound()
        {
            // Arrange
            var comment = new CommentUploadDto { Content = "Test comment content", PostId = 0 };
            var request = new UploadCommentCommand { Comment = comment, AuthUserId = 1 };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>().WithMessage("Post not found");
        }

        [Fact]
        public async Task UploadComment_ShouldThrowNotFoundException_WhenAuthorizedUserIsNotFound()
        {
            // Arrange
            var comment = new CommentUploadDto { Content = "Test comment content", PostId = 1 };
            var request = new UploadCommentCommand { Comment = comment, AuthUserId = 0 };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>().WithMessage("User not found");
        }
    }
}
