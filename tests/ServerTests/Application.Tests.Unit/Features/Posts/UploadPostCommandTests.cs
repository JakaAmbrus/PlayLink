using Application.Exceptions;
using Application.Features.Posts.Common;
using Application.Features.Posts.UploadPost;
using Application.Interfaces;
using Application.Models;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Tests.Unit.Features.Posts
{
    public class UploadPostCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public UploadPostCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            _photoService = Substitute.For<IPhotoService>();
            _cacheInvalidationService = Substitute.For<ICacheInvalidationService>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<UploadPostCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new UploadPostCommandHandler(
                    _context, _photoService, _cacheInvalidationService)
                    .Handle(c.Arg<UploadPostCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1 });
            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task UploadPost_ShouldCreatePostWithoutPhoto_WhenPhotoIsNotProvided()
        {
            // Arrange
            var request = new UploadPostCommand
            {
                AuthUserId = 1,
                PostContentDto = new PostContentDto { Description = "Test Description" }
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.PostDto.Should().NotBeNull();
            response.PostDto.Should().BeOfType<PostDto>();
            response.PostDto.PhotoUrl.Should().BeNull();
        }

        [Fact]
        public async Task UploadPost_ShouldCreatePostWithPhoto_WhenPhotoIsProvided()
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
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.PostDto.Should().NotBeNull();
            response.PostDto.Should().BeOfType<PostDto>();
            response.PostDto.PhotoUrl.Should().Be(mockPhotoResult.Url);
            response.PostDto.IsAuthorized.Should().BeTrue();
        }

        [Fact]
        public async Task UploadPost_ShouldThrowUnauthorizedException_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var request = new UploadPostCommand
            {
                AuthUserId = 2,
                PostContentDto = new PostContentDto { Description = "Test Description" }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Authenticated user not found");
        }

        [Fact]
        public async Task UploadPost_ShouldThrowServerErrorException_WhenPhotoServiceReturnsError()
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
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<ServerErrorException>()
                .WithMessage("Test Error");
        }
    }
}
