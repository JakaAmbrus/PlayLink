namespace WebAPI.Tests.Integration.Controllers.Admin
{
    public class EditRolesTests : BaseIntegrationTest
    {
        public EditRolesTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        private async Task InitializeTestSeedDataAsync()
        {
            Context.Users.Add(new AppUser { Id = 1, UserName = "tester" });
            Context.Users.Add(new AppUser { Id = 2, UserName = "modtester4" });
            await Context.SaveChangesAsync(CancellationToken.None);
        }

        private async Task InitializeTestAsync(List<string> roles)
        {
            await ResetDatabaseAsync();
            await InitializeAuthenticatedClient(roles);
            await InitializeTestSeedDataAsync();
        }
    }
}
