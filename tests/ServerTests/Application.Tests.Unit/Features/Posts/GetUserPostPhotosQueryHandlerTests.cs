using Application.Exceptions;
using Application.Features.Posts.GetUserPostPhotos;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Tests.Unit.Features.Posts
{
    public class GetUserPostPhotosQueryHandlerTests
    {
        private readonly GetUserPostPhotosQueryHandler _handler;
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public GetUserPostPhotosQueryHandlerTests()
        {
            _context = TestBase.CreateTestDbContext();
            _memoryCache = Substitute.For<IMemoryCache>();
            _handler = new GetUserPostPhotosQueryHandler(_context, _memoryCache);

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
        public async Task Handle_ShouldReturnPhotos_WhenUserExistsAndCacheDoesNotExist()
        {
            // Arrange
            var request = new GetUserPostPhotosQuery { Username = "Tester1" };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Photos.Should().NotBeEmpty();
            response.Photos.Should().AllBeOfType<string>();
        }

        [Fact]
        public async Task Handle_ShouldReturnPhotosFromCache_WhenCacheExists()
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
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Photos.Should().BeEquivalentTo(cachedPhotos);
        }

        [Fact]
        public async Task Handle_ShouldReturnNoPhotos_WhenCacheDoesNotExistAndUserHasNoPhotos()
        {
            // Arrange
            var request = new GetUserPostPhotosQuery { Username = "Tester2" };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Photos.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new GetUserPostPhotosQuery {  Username = "ImaginaryUser" };

            // Act
            var action = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User with username ImaginaryUser does not exist");
        }
    }
}
