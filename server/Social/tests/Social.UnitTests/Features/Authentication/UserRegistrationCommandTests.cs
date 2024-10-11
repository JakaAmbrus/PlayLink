using Social.Application.Features.Authentication.UserRegistration;
using Social.Application.Interfaces;
using Social.Domain.Entities;
using MediatR;
using Social.UnitTests.Configurations;

namespace Social.UnitTests.Features.Authentication
{
    public class UserRegistrationCommandTests
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IUserManager _userManager;
        private readonly ITokenService _tokenService;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public UserRegistrationCommandTests()
        {
            _context = TestBase.CreateTestDbContext();
            _userManager = Substitute.For<IUserManager>();
            _tokenService = Substitute.For<ITokenService>();
            _cacheInvalidationService = Substitute.For<ICacheInvalidationService>();
            var mediatorMock = Substitute.For<IMediator>();
            _mediator = mediatorMock;

            mediatorMock.Send(Arg.Any<UserRegistrationCommand>(), Arg.Any<CancellationToken>())
                .Returns(c => new UserRegistrationCommandHandler(_context, _userManager, _tokenService, _cacheInvalidationService)
                .Handle(c.Arg<UserRegistrationCommand>(), c.Arg<CancellationToken>()));

            SeedTestData(_context);
        }

        private static void SeedTestData(IApplicationDbContext context)
        {
            context.Users.Add(new AppUser { Id = 1, UserName = "tester" });

            context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        //While making tests for user registration I realized that EF Core InMemory database
        //does not support transactions. I will test this functionallity in integration tests with docker Postgres database.
    }
}
