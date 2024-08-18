using Application.Features.MessageGroups.Common;
using Application.Features.MessageGroups.GetGroupForConnection;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Tests.Unit.Features.MessageGroups
{
    public class GetGroupForConnectionQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public GetGroupForConnectionQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetGroupForConnectionQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetGroupForConnectionQueryHandler(_context)
                .Handle(c.Arg<GetGroupForConnectionQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Groups.Add(new Group 
            {
                Name = "test group",
                Connections = new List<Connection>
                {
                    new Connection { ConnectionId = "test connection", Username = "tester" }
                }
            });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetGroupForConnection_ShouldReturnGroupDtoWithConnection_WhenGroupExists()
        {
            // Arrange
            var request = new GetGroupForConnectionQuery
            {
                ConnectionId = "test connection"
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<GroupDto>();
            response.Name.Should().Be("test group");
            response.Connections.Should().NotBeEmpty();
            response.Connections[0].ConnectionId.Should().Be("test connection");
            response.Connections[0].Username.Should().Be("tester");
        }

        [Fact]
        public async Task GetGroupForConnection_ShouldThrowNotFoundException_WhenGroupDoesNotExist()
        {
            // Arrange
            var request = new GetGroupForConnectionQuery
            {
                ConnectionId = "test connection 2"
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Group not found");
        }
    }
}
