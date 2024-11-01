using Microsoft.EntityFrameworkCore;

namespace Social.UnitTests.Configurations
{
    public static class TestBase
    {
        public static TestSocialDbContext CreateTestDbContext()
        {
            var options = new DbContextOptionsBuilder<TestSocialDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TestSocialDbContext(options);
        }
    }
}
