using Application.Features.Users.GetUserIdFromUsername;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Tests.Unit.Features.Users
{
    public class GetUserIdByUsernameQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public GetUserIdByUsernameQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetUserIdByUsernameQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetUserIdByUsernameQueryHandler(_context)
                .Handle(c.Arg<GetUserIdByUsernameQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser
            {
                Id = 1,
                UserName = "tester",
            });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetUserIdByUsername_ShouldReturnUserId_WhenUserExists()
        {
            // Arrange
            var request = new GetUserIdByUsernameQuery { Username = "tester" };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public async Task GetUserIdByUsername_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new GetUserIdByUsernameQuery { Username = "tester2" };

            // Act
            Func<Task> action = async () => await _mediator.Send(request);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User not found");
        }
    }
}
