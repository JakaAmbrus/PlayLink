using Microsoft.EntityFrameworkCore;

namespace Social.UnitTests.Configurations
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
