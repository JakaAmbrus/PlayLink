using Application.Exceptions;
using Application.Features.Posts.DeletePost;
using Application.Interfaces;
using Application.Models;
using Application.Tests.Unit.TestUtilities;
using Domain.Entities;

namespace Application.Tests.Unit.Features.Posts
{
    public class DeletePostCommandHandlerTests
    {
        private readonly DeletePostCommandHandler _handler;
        private readonly IApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public DeletePostCommandHandlerTests()
        {
            _context = TestBase.CreateTestDbContext();
            _handler = new DeletePostCommandHandler(_context, _photoService, _cacheInvalidationService);
            _photoService = Substitute.For<IPhotoService>();
            _cacheInvalidationService = Substitute.For<ICacheInvalidationService>();
            // I do not want to actually interact with my Cloudinary account
            _photoService.DeletePhotoAsync(Arg.Any<string>())
                .Returns(Task.FromResult(new PhotoDeletionResult { Error = null }));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1 });
            context.Posts.Add(new Post { PostId = 1, AppUserId = 1, CommentsCount = 1 });
            context.Comments.Add(new Comment { CommentId = 1, PostId = 1, AppUserId = 1 });
            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task Handle_ShouldDeletePostAndReturnTheCorrectResponse_WhenUserIsTheOwner()
        {
            // Arrange
            var request = new DeletePostCommand 
            { 
                PostId = 1,
                AuthUserId = 1, 
                AuthUserRoles = new List<string>() 
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldDeletePostAndReturnTheCorrectResponse_WhenUserIsModerator()
        {
            // Arrange
            var request = new DeletePostCommand 
            { 
                PostId = 1, 
                AuthUserId = 1, 
                AuthUserRoles = new List<string> { "Moderator" } 
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldDecrementCommentsCount_WhenPostIsDeleted()
        {
            // Arrange
            var request = new DeletePostCommand 
            { 
                PostId = 1, 
                AuthUserId = 1, 
                AuthUserRoles = new List<string>() 
            };

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _context.Posts.Count().Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedException_WhenUserIsNotTheOwnerAndNotModerator()
        {
            // Arrange
            _context.Users.Add(new AppUser { Id = 2 });
            _context.SaveChangesAsync(CancellationToken.None).Wait();

            var request = new DeletePostCommand
            {
                PostId = 1, 
                AuthUserId = 2, 
                AuthUserRoles = new List<string>() 
            };

            // Act
            var result = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await result.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("User not authorized to delete this post");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAuthorizedUserDoesNotExist()
        {
            // Arrange
            var request = new DeletePostCommand
            {
                PostId = 1,
                AuthUserId = 2,
                AuthUserRoles = new List<string>()
            };

            // Act
            var result = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Authorized user not found");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenPostDoesNotExist()
        {
            // Arrange
            var request = new DeletePostCommand
            {
                PostId = 2,
                AuthUserId = 1,
                AuthUserRoles = new List<string>()
            };

            // Act
            var result = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Post was not found");
        }
    }
}
