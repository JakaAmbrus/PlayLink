using Social.Application.Features.Moderator.DeleteUserDescription;
using Social.Application.Interfaces;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;
using Social.UnitTests.Configurations;

namespace Social.UnitTests.Features.Moderator
{
    public class DeleteUserDescriptionCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public DeleteUserDescriptionCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<DeleteUserDescriptionCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new DeleteUserDescriptionCommandHandler(_context)
                .Handle(c.Arg<DeleteUserDescriptionCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { UserName = "Tester", Description = "Test description" });
            context.Users.Add(new AppUser { UserName = "NoDescription" });
            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task DeleteUserDescription_ShouldDeleteUserDescriptionAndReturnTheCorrectResponse_WhenUserIsModerator()
        {
            // Arrange
            var request = new DeleteUserDescriptionCommand { Username = "Tester" };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsDeleted.Should().BeTrue();
            _context.Users.Should().NotContain(u => u.Description == "Test description");
        }

        [Fact]
        public async Task DeleteUserDescription_ShouldReturnFalse_WhenUserDoesNotHaveDescription()
        {
            // Arrange
            var request = new DeleteUserDescriptionCommand { Username = "NoDescription" };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.IsDeleted.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteUserDescription_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new DeleteUserDescriptionCommand { Username = "ImaginaryUser" };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User not found.");
        }
    }
}
