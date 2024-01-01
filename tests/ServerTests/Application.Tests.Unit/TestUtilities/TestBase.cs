using Application.Tests.Unit.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Application.Tests.Unit.TestUtilities
{
    public static class TestBase
    {
        public static TestApplicationDbContext CreateTestDbContext()
        {
            var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TestApplicationDbContext(options);
        }
    }
}
