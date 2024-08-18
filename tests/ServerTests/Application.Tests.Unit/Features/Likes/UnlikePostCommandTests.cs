using Application.Features.Likes.UnlikePost;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Tests.Unit.Features.Likes
{
    public class UnlikePostCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public UnlikePostCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<UnlikePostCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new UnlikePostCommandHandler(_context)
                                              .Handle(c.Arg<UnlikePostCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1 });

            context.Posts.Add(new Post { PostId = 1, LikesCount = 1 });
            context.Posts.Add(new Post { PostId = 2, LikesCount = 0 });

            context.Likes.Add(new Like { PostId = 1, AppUserId = 1 });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task UnlikePost_ShouldUnlikePostAndDecrementLikesCount_WhenPostExistsAndIsLikedByUser()
        {
            // Arrange
            var request = new UnlikePostCommand { PostId = 1, AuthUserId = 1 };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Unliked.Should().BeTrue();
            _context.Likes.Count().Should().Be(0);
            _context.Posts.Find(1).LikesCount.Should().Be(0);
        }

        [Fact]
        public async Task UnlikePost_ShouldThrowNotFoundException_WhenPostDoesNotExist()
        {
            // Arrange
            var request = new UnlikePostCommand { PostId = 3, AuthUserId = 1 };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Post not found");
        }

        [Fact]
        public async Task UnlikePost_ShouldThrowNotFoundException_WhenPostIsNotLikedByUser()
        {
            // Arrange
            var request = new UnlikePostCommand { PostId = 2, AuthUserId = 1 };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Posts like not found");
        }
    }
}
