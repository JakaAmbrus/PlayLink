using Social.Application.Features.MessageGroups.GetConnection;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;

namespace Social.Application.Tests.Unit.Features.MessageGroups
{
    public class GetConnectionQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public GetConnectionQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetConnectionQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetConnectionQueryHandler(_context)
                .Handle(c.Arg<GetConnectionQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Connections.Add(new Connection { ConnectionId = "1", Username = "Tester" });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetConnection_ShouldReturnConnection_WhenInputsAreValid()
        {
            // Arrange
            var request = new GetConnectionQuery
            {
                ConnectionId = "1",
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<Connection>();
        }

        [Fact]
        public async Task GetConnection_ShouldThrowNotFoundException_WhenConnectionDoesNotExist()
        {
            // Arrange
            var request = new GetConnectionQuery
            {
                ConnectionId = "2",
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Connection not found");
        }
    }
}
