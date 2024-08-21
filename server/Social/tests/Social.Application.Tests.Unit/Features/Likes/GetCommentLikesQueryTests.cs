using Social.Application.Features.Likes.Common;
using Social.Application.Features.Likes.GetCommentLikes;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;

namespace Social.Application.Tests.Unit.Features.Likes
{
    public class GetCommentLikesQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public GetCommentLikesQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetCommentLikesQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetCommentLikesQueryHandler(_context)
                .Handle(c.Arg<GetCommentLikesQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            var users = Enumerable.Range(1, 11)
                .Select(i => new AppUser 
                { 
                    Id = i,
                    UserName = $"{i}",
                    FullName = $"{i} Tester",
                }).ToList();
            context.Users.AddRange(users);

            context.Posts.Add(new Post { PostId = 1, CommentsCount = 1 });

            context.Comments.Add(new Comment { CommentId = 1, PostId = 1, AppUserId = 1, LikesCount = 10 });
            context.Comments.Add(new Comment { CommentId = 2, PostId = 1, AppUserId = 2, LikesCount = 0 });

            var likes = Enumerable.Range(1, 10)
                .Select(i => new Like
                {
                    AppUserId = i,
                    CommentId = 1,
                }).ToList();
            context.Likes.AddRange(likes);

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetCommentLikes_ShouldReturnUsersThatLikedTheComment_WhenCommentExists()
        {
            // Arrange
            var request = new GetCommentLikesQuery { CommentId = 1, AuthUserId = 11 };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.LikedUsers.Should().NotBeNull();
            response.LikedUsers.Should().AllBeOfType<LikedUserDto>();
            response.LikedUsers.Count.Should().Be(10);
            response.LikedUsers[0].FullName.Should().Be("1 Tester");
            response.LikedUsers[0].Username.Should().Be("1");
        }

        [Fact]
        public async Task GetCommentLikes_ShouldReturnEmptyList_WhenCommentHasNoLikes()
        {
            // Arrange
            var request = new GetCommentLikesQuery { CommentId = 2, AuthUserId = 11 };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.LikedUsers.Should().NotBeNull();
            response.LikedUsers.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCommentLikes_ShouldReturnUsersThatLikedTheCommentWithoutAuthorizedUser_WhenCommentExists()
        {
            // Arrange
            var request = new GetCommentLikesQuery { CommentId = 1, AuthUserId = 1 };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.LikedUsers.Should().NotBeNull();
            response.LikedUsers.Should().AllBeOfType<LikedUserDto>();
            response.LikedUsers.Count.Should().Be(9);
            response.LikedUsers[0].FullName.Should().Be("2 Tester");
            response.LikedUsers[0].Username.Should().Be("2");
        }

        [Fact]
        public async Task GetCommentLikes_ShouldThrowNotFoundException_WhenCommentDoesNotExist()
        {
            // Arrange
            var request = new GetCommentLikesQuery { CommentId = 0, AuthUserId = 11 };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Comment not found");
        }
    }
}
