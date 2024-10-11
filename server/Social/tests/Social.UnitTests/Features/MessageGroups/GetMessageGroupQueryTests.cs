using Social.Application.Features.MessageGroups.Common;
using Social.Application.Features.MessageGroups.GetMessageGroup;
using Social.Application.Interfaces;
using Social.Domain.Entities;
using MediatR;
using Social.UnitTests.Configurations;

namespace Social.UnitTests.Features.MessageGroups
{
    public class GetMessageGroupQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

                public GetMessageGroupQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetMessageGroupQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetMessageGroupQueryHandler(_context)
                .Handle(c.Arg<GetMessageGroupQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Groups.Add(new Group { Name = "test group" });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetMessageGroup_ShouldReturnGroupDto_WhenGroupExists()
        {
            // Arrange
            var request = new GetMessageGroupQuery
            {
                GroupName = "test group"
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<GroupDto>();
            response.Name.Should().Be("test group");
            response.Connections.Should().BeEmpty();
        }

        [Fact]
        public async Task GetMessageGroup_ShouldReturnNull_WhenGroupDoesNotExist()
        {
            // Arrange
            var request = new GetMessageGroupQuery
            {
                GroupName = "test group 2"
            };

            // Act
            var response = await _mediator.Send(request, CancellationToken.None);

            // Assert
            response.Should().BeNull();
        }
    }
}
