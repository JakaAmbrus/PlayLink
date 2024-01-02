using Application.Exceptions;
using Application.Features.Moderator.DeleteUserPhoto;
using Application.Interfaces;
using Application.Models;
using Application.Tests.Unit.Configurations;
using Domain.Entities;

namespace Application.Tests.Unit.Features.Moderator
{
    public class DeleteUserPhotoCommandHandlerTests
    {
        private readonly DeleteUserPhotoCommandHandler _handler;
        private readonly IApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public DeleteUserPhotoCommandHandlerTests()
        {
            _context = TestBase.CreateTestDbContext();
            _photoService = Substitute.For<IPhotoService>();
            _cacheInvalidationService = Substitute.For<ICacheInvalidationService>();
            _handler = new DeleteUserPhotoCommandHandler(_context, _photoService, _cacheInvalidationService);

            _photoService.DeletePhotoAsync(Arg.Any<string>())
                .Returns(Task.FromResult(new PhotoDeletionResult { Error = null }));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser
            { 
                UserName = "Tester",
                ProfilePictureUrl = "picture_url",
                ProfilePicturePublicId = "picture_id" 
            });
            context.Users.Add(new AppUser { UserName = "NoProfilePicture" });
            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task Handle_ShouldDeleteUserPhoto_WhenPhotoExists()
        {
            // Arrange
            var request = new DeleteUserPhotoCommand { Username = "Tester" };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsDeleted.Should().BeTrue();
            _context.Users.First().ProfilePictureUrl.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenUserNotFound()
        {
            // Arrange
            var request = new DeleteUserPhotoCommand { Username = "ImaginaryUser" };

            // Act
            Func<Task> action = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User not found.");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenPhotoDoesNotExist()
        {
            // Arrange
            var request = new DeleteUserPhotoCommand { Username = "NoProfilePicture" };

            // Act
            Func<Task> action = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User does not have a profile picture.");
        }

        [Fact]
        public async Task Handle_ShouldThrowServerErrorException_WhenPhotoDeletionFails()
        {
            // Arrange
            _photoService.DeletePhotoAsync(Arg.Any<string>())
                .Returns(Task.FromResult(new PhotoDeletionResult { Error = "Error" }));

            var request = new DeleteUserPhotoCommand { Username = "Tester" };

            // Act
            Func<Task> action = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<ServerErrorException>();
        }
    }
}
