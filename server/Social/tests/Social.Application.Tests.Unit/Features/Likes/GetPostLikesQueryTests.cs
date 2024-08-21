using Social.Application.Features.Likes.Common;
using Social.Application.Features.Likes.GetPostLikes;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;

namespace Social.Application.Tests.Unit.Features.Likes
{
    public class GetPostLikesQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public GetPostLikesQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetPostLikesQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetPostLikesQueryHandler(_context)
                .Handle(c.Arg<GetPostLikesQuery>(), c.Arg<CancellationToken>()));

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

            context.Posts.Add(new Post { PostId = 1, LikesCount = 10 });
            context.Posts.Add(new Post { PostId = 2, LikesCount = 0 });

            var likes = Enumerable.Range(1, 10)
                .Select(i => new Like
                {
                    AppUserId = i,
                    PostId = 1,
                }).ToList();
            context.Likes.AddRange(likes);

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetPostLikes_ShouldReturnUsersThatLikedThePost_WhenPostExists()
        {
            // Arrange
            var request = new GetPostLikesQuery { PostId = 1, AuthUserId = 11 };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.LikedUsers.Should().NotBeNull();
            response.LikedUsers.Should().BeOfType<List<LikedUserDto>>();
            response.LikedUsers.Count.Should().Be(10);
            response.LikedUsers[0].FullName.Should().Be("1 Tester");
            response.LikedUsers[0].Username.Should().Be("1");
        }

        [Fact]
        public async Task GetPostLikes_ShouldReturnEmptyList_WhenPostHasNoLikes()
        {
            // Arrange
            var request = new GetPostLikesQuery { PostId = 2, AuthUserId = 11 };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.LikedUsers.Should().NotBeNull();
            response.LikedUsers.Should().BeOfType<List<LikedUserDto>>();
            response.LikedUsers.Should().BeEmpty();
        }

        [Fact]
        public async Task GetPostLikes_ShouldReturnUsersThatLikedTheCommentWithoutAuthorizedUser_WhenPostExists()
        {
            // Arrange
            var request = new GetPostLikesQuery { PostId = 1, AuthUserId = 1 };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.LikedUsers.Should().NotBeNull();
            response.LikedUsers.Should().BeOfType<List<LikedUserDto>>();
            response.LikedUsers.Count.Should().Be(9);
            response.LikedUsers[0].FullName.Should().Be("2 Tester");
            response.LikedUsers[0].Username.Should().Be("2");
        }

        [Fact]
        public async Task GetPostLikes_ShouldThrowNotFoundException_WhenPostDoesNotExist()
        {
            // Arrange
            var request = new GetPostLikesQuery { PostId = 3, AuthUserId = 11 };

            // Act
            Func<Task> act = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}

