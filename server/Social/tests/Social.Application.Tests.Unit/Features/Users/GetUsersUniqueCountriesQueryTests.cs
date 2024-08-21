using Social.Application.Features.Users.GetUsersUniqueCountries;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Domain.Entities;
using MediatR;

namespace Social.Application.Tests.Unit.Features.Users
{
    public class GetUsersUniqueCountriesQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public GetUsersUniqueCountriesQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetUsersUniqueCountriesQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetUsersUniqueCountriesQueryHandler(_context)
                .Handle(c.Arg<GetUsersUniqueCountriesQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser
            {
                Id = 1,
                UserName = "tester",
                Country = "Slovenia"
            });
            context.Users.Add(new AppUser
            {
                Id = 2,
                UserName = "tester2",
                Country = "Romania"
            });
            context.Users.Add(new AppUser
            {
                Id = 3,
                UserName = "tester3",
                Country = "Austria"
            });
            context.Users.Add(new AppUser
            {
                Id = 4,
                UserName = "tester4",
                Country = "Switzerland"
            });
            context.Users.Add(new AppUser
            {
                Id = 5,
                UserName = "tester5",
                Country = "USA"
            });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetUsersUniqueCountries_ShouldReturnUniqueCountriesFromUsers_WhenCalled()
        {
            // Arrange
            _context.Users.Add(new AppUser
            {
                Id = 6,
                UserName = "tester6",
                Country = "Slovenia"
            });
            var request = new GetUsersUniqueCountriesQuery { AuthUserId = 6 };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            result.Countries.Should().HaveCount(5);
            result.Countries.Should().Contain("Slovenia");
            result.Countries.Should().Contain("Romania");
            result.Countries.Should().Contain("Austria");
            result.Countries.Should().Contain("Switzerland");
            result.Countries.Should().Contain("USA");
        }

        [Fact]
        public async Task GetUsersUniqueCountries_ShouldReturnUniqueCountriesFromUsersExeptCurrent_WhenCalled()
        {
            // Arrange
            _context.Users.Add(new AppUser
            {
                Id = 6,
                UserName = "tester6",
                Country = "China"
            });
            var request = new GetUsersUniqueCountriesQuery { AuthUserId = 6 };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            result.Countries.Should().HaveCount(5);
            result.Countries.Should().Contain("Slovenia");
            result.Countries.Should().Contain("Romania");
            result.Countries.Should().Contain("Austria");
            result.Countries.Should().Contain("Switzerland");
            result.Countries.Should().Contain("USA");
            result.Countries.Should().NotContain("China");
        }

        [Fact]
        public async Task GetUsersUniqueCountries_ShouldReturnEmptyList_WhenNoUsersInDatabase()
        {
            // Arrange
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync(CancellationToken.None);

            var request = new GetUsersUniqueCountriesQuery { AuthUserId = 1 };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            result.Countries.Should().HaveCount(0);
        }
    }
}
