using Application.Features.Moderator.DeleteUserPhoto;
using Application.Interfaces;
using Application.Models;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Tests.Unit.Features.Moderator
{
    public class DeleteUserPhotoCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public DeleteUserPhotoCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            _photoService = Substitute.For<IPhotoService>();
            _cacheInvalidationService = Substitute.For<ICacheInvalidationService>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<DeleteUserPhotoCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new DeleteUserPhotoCommandHandler(
                    _context, _photoService, _cacheInvalidationService)
                    .Handle(c.Arg<DeleteUserPhotoCommand>(), c.Arg<CancellationToken>()));

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
        public async Task DeleteUserPhoto_ShouldDeleteUserPhoto_WhenPhotoExists()
        {
            // Arrange
            var request = new DeleteUserPhotoCommand { Username = "Tester" };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.IsDeleted.Should().BeTrue();
            _context.Users.First().ProfilePictureUrl.Should().BeNull();
        }

        [Fact]
        public async Task DeleteUserPhoto_ShouldThrowNotFoundException_WhenUserNotFound()
        {
            // Arrange
            var request = new DeleteUserPhotoCommand { Username = "ImaginaryUser" };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User not found.");
        }

        [Fact]
        public async Task DeleteUserPhoto_ShouldThrowNotFoundException_WhenPhotoDoesNotExist()
        {
            // Arrange
            var request = new DeleteUserPhotoCommand { Username = "NoProfilePicture" };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User does not have a profile picture.");
        }

        [Fact]
        public async Task DeleteUserPhoto_ShouldThrowServerErrorException_WhenPhotoDeletionFails()
        {
            // Arrange
            _photoService.DeletePhotoAsync(Arg.Any<string>())
                .Returns(Task.FromResult(new PhotoDeletionResult { Error = "Error" }));

            var request = new DeleteUserPhotoCommand { Username = "Tester" };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<ServerErrorException>();
        }
    }
}
