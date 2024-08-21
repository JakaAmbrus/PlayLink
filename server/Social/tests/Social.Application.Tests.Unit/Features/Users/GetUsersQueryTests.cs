using Social.Application.Features.Users.GetUsers;
using Social.Application.Interfaces;
using Social.Application.Tests.Unit.Configurations;
using Social.Application.Utils;
using Social.Domain.Entities;
using MediatR;

namespace Social.Application.Tests.Unit.Features.Users
{
    public class GetUsersQueryTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public GetUsersQueryTests()
        {
            _context = TestBase.CreateTestDbContext();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<GetUsersQuery>(), Arg.Any<CancellationToken>())
                .Returns(c => new GetUsersQueryHandler(_context)
                .Handle(c.Arg<GetUsersQuery>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        // In my seed data I have a a variety of users with different characteristics that I can filter through
        private static void SeedTestData(IApplicationDbContext context)
        {
            var elderMaleUsersUSA = Enumerable.Range(1, 10).Select(i => new AppUser
            {
                Id = i,
                UserName = $"tester{i}",
                FullName = $"tester {i}",
                DateOfBirth = new DateTime(1950, 5, 16),
                Country = "USA",
                Gender = "male"
            }).ToList();
            context.Users.AddRange(elderMaleUsersUSA);

            var youngFemaleUsersUK = Enumerable.Range(11, 10).Select(i => new AppUser
            {
                Id = i,
                UserName = $"tester{i}",
                FullName = $"tester {i}",
                DateOfBirth = new DateTime(2005, 5, 16),
                Country = "UK",
                Gender = "female"
            }).ToList();
            context.Users.AddRange(youngFemaleUsersUK);

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetUsers_ShouldReturnAllUsersExceptCurrent_WhenPageSizeIsBiggerThanNumberOfUsers()
        {
            // Arrange
            var request = new GetUsersQuery 
            { 
                AuthUserId = 1,
                Params = new UserParams 
                { 
                    PageNumber = 1,
                    PageSize = 25
                }
            };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            result.Users.Count.Should().Be(19);
            result.Users.Should().NotContain(u => u.AppUserId == 1);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnPagedListOfUsersExceptCurrent_WhenPageSizeIsSmallerThanNumberOfUsers()
        {
            // Arrange
            var request = new GetUsersQuery
            {
                AuthUserId = 1,
                Params = new UserParams
                {
                    PageNumber = 1,
                    PageSize = 10
                }
            };

            // Act
            var result = await _mediator.Send(request);

            // Assert
            result.Users.Count.Should().Be(10);
            result.Users.Should().NotContain(u => u.AppUserId == 1);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnPagedListOfUsersOrderedByLastActive_WhenCalledWithLastActiveFilter()
        {
            // Arrange
            var request = new GetUsersQuery
            {
                AuthUserId = 1,
                Params = new UserParams
                {
                    PageNumber = 1,
                    PageSize = 5,
                    OrderBy = "lastActive"
                }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Users.Count.Should().Be(5);
            var users = response.Users.ToList();

            var orderOfUserIds = _context.Users
                .OrderByDescending(u => u.LastActive)
                .Select(u => u.Id)
                .Take(users.Count)
                .ToList();

            users.Select(u => u.AppUserId).Should().Equal(orderOfUserIds);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnPagedListOfUsersOrderedByCreated_WhenCalledWithCreatedFilter()
        {
            // Arrange
            var request = new GetUsersQuery
            {
                AuthUserId = 1,
                Params = new UserParams
                {
                    PageNumber = 1,
                    PageSize = 5,
                    OrderBy = "created"
                }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Users.Count.Should().Be(5);
            var users = response.Users.ToList();

            var orderOfUserIds = _context.Users
                .OrderByDescending(u => u.Created)
                .Select(u => u.Id)
                .Take(users.Count)
                .ToList();

            users.Select(u => u.AppUserId).Should().Equal(orderOfUserIds);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnAPagedListOfElderUsers_WhenCalledWithElderFilter()
        {
            // Arrange
            var request = new GetUsersQuery
            {
                AuthUserId = 1,
                Params = new UserParams
                {
                    PageNumber = 1,
                    PageSize = 5,
                    MinAge = 50,
                    MaxAge = 100
                }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Users.Count.Should().Be(5);
            response.Users.Should().OnlyContain(u => u.Age >= 50 && u.Age <= 100);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnAPagedListOfYoungUsers_WhenCalledWithYoungFilter()
        {
            // Arrange
            var request = new GetUsersQuery
            {
                AuthUserId = 1,
                Params = new UserParams
                {
                    PageNumber = 1,
                    PageSize = 5,
                    MinAge = 12,
                    MaxAge = 30
                }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Users.Count.Should().Be(5);
            response.Users.Should().OnlyContain(u => u.Age >= 12 && u.Age <= 30);
        }

        [Theory]
        [InlineData("USA")]
        [InlineData("UK")]
        public async Task GetUsers_ShouldReturnAPagedListOfUsersByCountry_WhenCalledWithCountryFilter(
            string country)
        {
            // Arrange
            var request = new GetUsersQuery
            {
                AuthUserId = 1,
                Params = new UserParams
                {
                    PageNumber = 1,
                    PageSize = 5,
                    Country = country
                }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Users.Count.Should().Be(5);
            response.Users.Should().OnlyContain(u => u.Country == country);
        }

        [Theory]
        [InlineData("male")]
        [InlineData("female")]
        public async Task GetUsers_ShouldReturnAPagedListOfUsersByGender_WhenCalledWithGenderFilter(
            string gender)
        {
            // Arrange
            var request = new GetUsersQuery
            {
                AuthUserId = 1,
                Params = new UserParams
                {
                    PageNumber = 1,
                    PageSize = 5,
                    Gender = gender
                }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Users.Count.Should().Be(5);
            response.Users.Should().OnlyContain(u =>u.Gender == gender);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnEmptyList_WhenPageNumberExceedsTotalNumberOfPages()
        {
            // Arrange
            var request = new GetUsersQuery
            {
                AuthUserId = 1,
                Params = new UserParams
                {
                    PageNumber = 3,
                    PageSize = 10
                }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Users.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnEmptyList_WhenNoUsersInDatabase()
        {
            // Arrange
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync(CancellationToken.None);

            var request = new GetUsersQuery
            {
                AuthUserId = 1,
                Params = new UserParams
                {
                    PageNumber = 1,
                    PageSize = 10
                }
            };

            // Act
            var response = await _mediator.Send(request);

            // Assert
            response.Users.Count.Should().Be(0);
        }
    }
}
