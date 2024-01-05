using Application.Features.MessageGroups.AddGroup;
using Application.Features.MessageGroups.Common;
using Application.Interfaces;
using Application.Tests.Unit.Configurations;
using MediatR;

namespace Application.Tests.Unit.Features.MessageGroups
{
    public class AddGroupCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public AddGroupCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<AddGroupCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new AddGroupCommandHandler(_context)
                .Handle(c.Arg<AddGroupCommand>(), c.Arg<CancellationToken>()));
        }

        [Fact]
        public async Task AddGroup_ShouldReturnGroupDTO_WhenInputsAreValid()
        {
            // Arrange
            var request = new AddGroupCommand
            {
                GroupName = "Test Group",
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<GroupDto>();
        }
    }
}
