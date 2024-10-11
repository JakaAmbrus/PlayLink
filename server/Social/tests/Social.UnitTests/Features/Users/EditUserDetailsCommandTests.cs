using Social.Application.Features.Users.Common;
using Social.Application.Features.Users.EditUserDetails;
using Social.Application.Interfaces;
using Social.Application.Models;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Social.UnitTests.Configurations;

namespace Social.UnitTests.Features.Users
{
    public class EditUserDetailsCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly ICacheInvalidationService _cacheInvalidationService;
        
        public EditUserDetailsCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            _photoService = Substitute.For<IPhotoService>();
            _cacheInvalidationService = Substitute.For<ICacheInvalidationService>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<EditUserDetailsCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new EditUserDetailsCommandHandler(_context, _photoService, _cacheInvalidationService)
                .Handle(c.Arg<EditUserDetailsCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser 
            { 
                Id = 1,
                UserName = "tester",
                Description = "test description",
                Country = "Slovenia",
                ProfilePictureUrl = "https://res.cloudinary.com",
                ProfilePicturePublicId = "publicId"
            });
            context.Users.Add(new AppUser 
            { 
                Id = 2,
                UserName = "tester2",
                Description = "test description",
            });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task EditUserDetails_ShouldReturnEditUserDetailsResult_WhenUserEditsAllDetails()
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

            _photoService.DeletePhotoAsync(Arg.Any<string>())
               .Returns(Task.FromResult(new PhotoDeletionResult { Error = null }));

            var request = new EditUserDetailsCommand
            {
                AuthUserId = 1,
                EditUserDto = new EditUserDto
                {
                    Username = "tester",
                    Description = "test description edited",
                    Country = "Romania",
                    PhotoFile = mockPhotoFile
                }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<EditUserDetailsResult>();
            _context.Users.Find(1).Description.Should().Be("test description edited");
            _context.Users.Find(1).Country.Should().Be("Romania");
            _context.Users.Find(1).ProfilePictureUrl.Should().Be("http://test.com/photo.jpg");          
        }

        [Fact]
        public async Task EditUserDetails_ShouldReturnEditUserDetailsResult_WhenUserEditsDescription()
        {
            // Arrange
            var request = new EditUserDetailsCommand
            {
                AuthUserId = 1,
                EditUserDto = new EditUserDto
                {
                    Username = "tester",
                    Description = "test description edited",
                }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<EditUserDetailsResult>();
            _context.Users.Find(1).Description.Should().Be("test description edited");
        }

        [Fact]
        public async Task EditUserDetails_ShouldReturnEditUserDetailsResult_WhenUserEditsCountry()
        {
            // Arrange
            var request = new EditUserDetailsCommand
            {
                AuthUserId = 1,
                EditUserDto = new EditUserDto
                {
                    Username = "tester",
                    Country = "Romania"
                }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<EditUserDetailsResult>();
            _context.Users.Find(1).Country.Should().Be("Romania");
        }

        [Fact]
        public async Task EditUserDetails_ShouldReturnEditUserDetailsResult_WhenUserEditsPhoto()
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

            _photoService.DeletePhotoAsync(Arg.Any<string>())
               .Returns(Task.FromResult(new PhotoDeletionResult { Error = null }));

            var request = new EditUserDetailsCommand
            {
                AuthUserId = 1,
                EditUserDto = new EditUserDto
                {
                    Username = "tester",
                    PhotoFile = mockPhotoFile
                }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<EditUserDetailsResult>();
            _context.Users.Find(1).ProfilePictureUrl.Should().Be("http://test.com/photo.jpg");
        }

        [Fact]
        public async Task EditUserDetails_ShouldThrowNotFoundException_WhenUserNotFound()
        {
            // Arrange
            var request = new EditUserDetailsCommand
            {
                AuthUserId = 5,
                EditUserDto = new EditUserDto
                {
                    Username = "tester",
                    Description = "test description edited",
                    Country = "Romania"
                }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Authorized user was not found");
        }
        [Fact]
        public async Task EditUserDetails_ShouldThrowUnauthorizedException_WhenUserIsNotTheOwner()
        {
            // Arrange
            var request = new EditUserDetailsCommand
            {
                AuthUserId = 2,
                EditUserDto = new EditUserDto
                {
                    Username = "tester",
                    Description = "test description edited",
                }
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("User not authorized to edit profile");
        }
    }
}
