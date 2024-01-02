using Application.Exceptions;
using Application.Features.Posts.Common;
using Application.Features.Posts.UploadPost;
using Application.Interfaces;
using Application.Models;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Tests.Unit.Features.Posts
{
    public class UploadPostCommandHandlerTests
    {
        private readonly UploadPostCommandHandler _handler;
        private readonly IApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public UploadPostCommandHandlerTests()
        {
            _context = TestBase.CreateTestDbContext();
            _photoService = Substitute.For<IPhotoService>();
            _cacheInvalidationService = Substitute.For<ICacheInvalidationService>();
            _handler = new UploadPostCommandHandler(_context, _photoService, _cacheInvalidationService);

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1 });
            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task Handle_ShouldCreatePostWithoutPhoto_WhenPhotoNotProvided()
        {
            // Arrange
            var request = new UploadPostCommand
            {
                AuthUserId = 1,
                PostContentDto = new PostContentDto { Description = "Test Description" }
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.PostDto.Should().NotBeNull();
            result.PostDto.PhotoUrl.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldCreatePostWithPhoto_WhenPhotoProvided()
        {
            // Arrange
            var mockPhotoResult = new PhotoUploadResult
            {
                PublicId = "test_public_id",
                Url = "http://test.com/photo.jpg",
                Error = null
            };

            _photoService.AddPhotoAsync(Arg.Any<IFormFile>(), Arg.Any<string>())
                .Returns(Task.FromResult(mockPhotoResult));

            var mockPhotoFile = Substitute.For<IFormFile>();
            mockPhotoFile.FileName.Returns("test.jpg");
            mockPhotoFile.Length.Returns(1024);

            var request = new UploadPostCommand
            {
                AuthUserId = 1,
                PostContentDto = new PostContentDto
                {
                    Description = "Test Description",
                    PhotoFile = mockPhotoFile
                }
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.PostDto.PhotoUrl.Should().Be(mockPhotoResult.Url);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorizedException_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var request = new UploadPostCommand
            {
                AuthUserId = 2,
                PostContentDto = new PostContentDto { Description = "Test Description" }
            };

            // Act
            var result = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Authenticated user not found");
        }

        [Fact]
        public async Task Handle_ShouldThrowServerErrorException_WhenPhotoServiceReturnsError()
        {
            // Arrange
            var mockErrorResult = new PhotoUploadResult
            {
                Error = "Test Error"
            };

            _photoService.AddPhotoAsync(Arg.Any<IFormFile>(), Arg.Any<string>())
                .Returns(Task.FromResult(mockErrorResult));

            var mockPhotoFile = Substitute.For<IFormFile>();
            mockPhotoFile.FileName.Returns("test.jpg");
            mockPhotoFile.Length.Returns(1024);

            var request = new UploadPostCommand
            {
                AuthUserId = 1,
                PostContentDto = new PostContentDto
                {
                    Description = "Test Description",
                    PhotoFile = mockPhotoFile
                }
            };

            // Act
            Func<Task> action = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<ServerErrorException>()
                .WithMessage("Test Error");
        }
    }
}
