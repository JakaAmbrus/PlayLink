using Social.Application.Features.MessageGroups.RemoveConnection;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;

namespace Social.Application.Tests.Unit.Features.MessageGroups
{
    public class RemoveConnectionCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public RemoveConnectionCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<RemoveConnectionCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new RemoveConnectionCommandHandler(_context)
                .Handle(c.Arg<RemoveConnectionCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Connections.Add(new Connection { ConnectionId = "test connection", Username = "tester" });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task RemoveConnection_ShouldReturnRemoveConnection_WhenConnectionExists()
        {
            // Arrange
            var request = new RemoveConnectionCommand
            {
                ConnectionId = "test connection"
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<RemoveConnectionResponse>();
            response.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task RemoveConnection_ShouldThrowNotFoundException_WhenConnectionDoesNotExist()
        {
            // Arrange
            var request = new RemoveConnectionCommand
            {
                ConnectionId = "test connection 2"
            };

            // Act
            Func<Task> act = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Connection not found");
        }
    }
}
