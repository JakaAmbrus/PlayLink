using Application.Exceptions;
using Application.Features.Comments.Common;
using Application.Features.Comments.UploadComment;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;

namespace Application.Tests.Unit.Features.Comments
{
    public class UploadCommentCommandHandlerTests
    {
        private readonly UploadCommentCommandHandler _handler;
        private readonly IApplicationDbContext _context;

        public UploadCommentCommandHandlerTests()
        {
            _context = TestBase.CreateTestDbContext();
            _handler = new UploadCommentCommandHandler(_context);

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Posts.Add(new Post { PostId = 1, CommentsCount = 0 });
            context.Users.Add(new AppUser { Id = 1 });
            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task Handle_ShouldReturnCorrectCommentResponse_WhenCommentIsUploaded()
        {
            // Arrange
            var comment = new CommentUploadDto { Content = "Test comment content", PostId = 1 };
            var request = new UploadCommentCommand { Comment = comment, AuthUserId = 1 };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

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
        public async Task Handle_ShouldIncrementCommentsCount_WhenCommentIsUploaded()
        {
            // Arrange
            var comment = new CommentUploadDto { Content = "Test comment content", PostId = 1 };
            var request = new UploadCommentCommand { Comment = comment, AuthUserId = 1 };

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            var updatedPost = await _context.Posts.FindAsync(1);
            updatedPost.CommentsCount.Should().Be(1);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenPostIsNotFound()
        {
            // Arrange
            var comment = new CommentUploadDto { Content = "Test comment content", PostId = 0 };
            var request = new UploadCommentCommand { Comment = comment, AuthUserId = 1 };

            // Act
            var action = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>().WithMessage("Post not found");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAuthorizedUserIsNotFound()
        {
            // Arrange
            var comment = new CommentUploadDto { Content = "Test comment content", PostId = 1 };
            var request = new UploadCommentCommand { Comment = comment, AuthUserId = 0 };

            // Act
            var action = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>().WithMessage("User not found");
        }
    }
}
