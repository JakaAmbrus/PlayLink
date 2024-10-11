using Social.Application.Features.Posts.GetUserPostPhotos;
using Social.Application.Interfaces;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Social.UnitTests.Configurations;

namespace Social.UnitTests.Features.Posts
{
    public class GetUserPostPhotosQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public GetUserPostPhotosQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            _memoryCache = Substitute.For<IMemoryCache>();       
            var mediatorMock = Substitute.For<IMediator>(); 
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetUserPostPhotosQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetUserPostPhotosQueryHandler(_context, _memoryCache)
                .Handle(c.Arg<GetUserPostPhotosQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1, UserName = "Tester1", ProfilePictureUrl = "https://photo.com" });
            context.Users.Add(new AppUser { Id = 2, UserName = "Tester2" });

            var posts = Enumerable.Range(1, 10)
                .Select(i => new Post
                {
                    PostId = i,
                    AppUserId = 1,
                    DatePosted = DateTime.UtcNow.AddMinutes(-i),
                    PhotoUrl = $"https://photo{i}.com",
                }).ToList();
            context.Posts.AddRange(posts);

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetUserPostPhotos_ShouldReturnPhotos_WhenUserExistsAndCacheDoesNotExist()
        {
            // Arrange
            var request = new GetUserPostPhotosQuery { Username = "Tester1" };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Photos.Should().NotBeEmpty();
            response.Photos.Should().AllBeOfType<string>();
        }

        [Fact]
        public async Task GetUserPostPhotos_ShouldReturnPhotosFromCache_WhenCacheExists()
        {
            // Arrange
            var request = new GetUserPostPhotosQuery { Username = "Tester1" };
            var cachedPhotos = new List<string> { "https://cachedphoto.com" };
            _memoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<List<string>>()).Returns(x =>
            {
                x[1] = cachedPhotos;
                return true;
            });

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Photos.Should().BeEquivalentTo(cachedPhotos);
        }

        [Fact]
        public async Task GetUserPostPhotos_ShouldReturnNoPhotos_WhenCacheDoesNotExistAndUserHasNoPhotos()
        {
            // Arrange
            var request = new GetUserPostPhotosQuery { Username = "Tester2" };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Photos.Should().BeEmpty();
        }

        [Fact]
        public async Task GetUserPostPhotos_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new GetUserPostPhotosQuery {  Username = "ImaginaryUser" };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User with username ImaginaryUser does not exist");
        }
    }
}
