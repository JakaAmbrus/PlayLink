using Social.Application.Features.Likes.LikePost;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;

namespace Social.Application.Tests.Unit.Features.Likes
{
    public class LikePostCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public LikePostCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<LikePostCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new LikePostCommandHandler(_context)
                .Handle(c.Arg<LikePostCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1 });

            context.Posts.Add(new Post { PostId = 1, LikesCount = 0 });
            context.Posts.Add(new Post { PostId = 2, LikesCount = 1 });

            context.Likes.Add(new Like { AppUserId = 1, PostId = 2 });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task LikePost_ShouldAddLikeToPost_WhenPostExists()
        {
            // Arrange
            var request = new LikePostCommand { PostId = 1, AuthUserId = 1 };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().BeOfType<LikePostResponse>();
            response.Liked.Should().BeTrue();
            _context.Posts.Find(1).LikesCount.Should().Be(1);
        }

        [Fact]
        public async Task LikePost_ShouldThrowNotFoundException_WhenPostNotFound()
        {
            // Arrange
            var request = new LikePostCommand { PostId = 3, AuthUserId = 1 };

            // Act
            Func<Task> act = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Post not found");
        }

        [Fact]
        public async Task LikePost_ShouldThrowBadRequestException_WhenPostIsAlreadyLiked()
        {
            // Arrange
            var request = new LikePostCommand { PostId = 2, AuthUserId = 1 };

            // Act
            Func<Task> act = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("You have already liked this post");
        }
    }
}
