using Application.Features.Users.Common;
using Application.Features.Users.GetUserById;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Tests.Unit.Features.Users
{
    public class GetUserByIdQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public GetUserByIdQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetUserByIdQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetUserByIdQueryHandler(_context)
                .Handle(c.Arg<GetUserByIdQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1, UserName = "tester" });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            var request = new GetUserByIdQuery { Id = 1 };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.User.Should().BeOfType<UsersDto>();
        }

        [Fact]
        public async Task GetUserById_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new GetUserByIdQuery { Id = 2 };

            // Act
            Func<Task> act = async () => await _mediator.Send(request);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User with the id 2 not found.");
        }
    }
}
