using Social.Application.Features.MessageGroups.AddConnectionToGroup;
using Social.Application.Features.MessageGroups.Common;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;

namespace Social.Application.Tests.Unit.Features.MessageGroups
{
    public class AddConnectionToGroupCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public AddConnectionToGroupCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<AddConnectionToGroupCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new AddConnectionToGroupCommandHandler(_context)
                .Handle(c.Arg<AddConnectionToGroupCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Groups.Add(new Group { Name = "Test Group" });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task AddConnectionToGroup_ShouldReturnGroupDTO_WhenInputsAreValid()
        {
            // Arrange
            var request = new AddConnectionToGroupCommand
            {
                GroupName = "Test Group",
                ConnectionDto = new ConnectionDto
                {
                    ConnectionId = "1",
                    Username = "tester"
                }
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<GroupDto>();                 
        }

        [Fact]
        public async Task AddConnectionToGroup_ShouldThrowNotFoundException_WhenGroupDoesNotExist()
        {
            // Arrange
            var request = new AddConnectionToGroupCommand
            {
                GroupName = "Imaginary Group",
                ConnectionDto = new ConnectionDto
                {
                    ConnectionId = "1",
                    Username = "tester"
                }
            };

            // Act
            Func<Task> act = async () => await _mediator.Send(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Group not found");
        }
    }
}
