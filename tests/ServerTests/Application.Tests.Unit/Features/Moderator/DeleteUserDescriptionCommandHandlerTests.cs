using Application.Exceptions;
using Application.Features.Moderator.DeleteUserDescription;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;

namespace Application.Tests.Unit.Features.Moderator
{
    public class DeleteUserDescriptionCommandHandlerTests
    {
        private readonly DeleteUserDescriptionCommandHandler _handler;
        private readonly IApplicationDbContext _context;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public DeleteUserDescriptionCommandHandlerTests()
        {
            _context = TestBase.CreateTestDbContext();
            _cacheInvalidationService = Substitute.For<ICacheInvalidationService>();
            _handler = new DeleteUserDescriptionCommandHandler(_context, _cacheInvalidationService);

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { UserName = "Tester", Description = "Test description" });
            context.Users.Add(new AppUser { UserName = "NoDescription" });
            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task Handle_ShouldDeleteUserDescriptionAndReturnTheCorrectResponse_WhenUserIsModerator()
        {
            // Arrange
            var request = new DeleteUserDescriptionCommand { Username = "Tester" };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsDeleted.Should().BeTrue();
            _context.Users.Should().NotContain(u => u.Description == "Test description");
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenUserDoesNotHaveDescription()
        {
            // Arrange
            var request = new DeleteUserDescriptionCommand { Username = "NoDescription" };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new DeleteUserDescriptionCommand { Username = "ImaginaryUser" };

            // Act
            var result = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User not found.");
        }
    }
}
