using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace WebAPI.Tests.Integration.Configurations
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
    {
        private readonly IntegrationTestWebAppFactory _factory;
        private IServiceScope _scope;
        protected ISender Mediator;
        protected DataContext Context;
        protected IUserManager UserManager;
        protected RoleManager<AppRole> RoleManager;
        protected HttpClient Client;
        private ITokenService _tokenService;
        protected IPhotoService PhotoService;

        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _factory = factory;
            CreateNewScope();
        }

        private void CreateNewScope()
        {
            _scope?.Dispose();
            _scope = _factory.Services.CreateScope();
            InitializeServices(_scope.ServiceProvider);
        }

        private void InitializeServices(IServiceProvider services)
        {
            Mediator = services.GetRequiredService<ISender>();
            Context = services.GetRequiredService<DataContext>();
            UserManager = services.GetRequiredService<IUserManager>();
            RoleManager = services.GetRequiredService<RoleManager<AppRole>>();
            _tokenService = services.GetRequiredService<ITokenService>();
            PhotoService = services.GetRequiredService<IPhotoService>();
            Client = _factory.CreateClient();
        }

        protected void RefreshContext()
        {
            CreateNewScope();
        }

        protected async Task ResetDatabaseAsync()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<DataContext>();
                await db.Database.EnsureDeletedAsync();
                await db.Database.EnsureCreatedAsync();

                string[] roleNames = { "Member", "Moderator", "Admin", "Guest" };
                foreach (var roleName in roleNames)
                {
                    var roleExist = await RoleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        var roleResult = await RoleManager.CreateAsync(new AppRole { Name = roleName });

                        if (!roleResult.Succeeded)
                        {
                            throw new InvalidOperationException($"Error seeding '{roleName}' role");
                        }
                    }
                }
            }
        }

        protected async Task InitializeAuthenticatedClient(List<string> roles)
        {
            var user = new AppUser 
            { 
                Id = 1,
                UserName = "authtester",
                FullName = "Auth Tester",
                Country = "Slovenia",
                Gender = "male",
                Description = "I am a tester.",
                ProfilePictureUrl = "https://CloudinaryTestPicture.com",
                DateOfBirth = new DateOnly(1999, 5, 16),
                Created = DateTime.UtcNow.AddDays(-1)
            };

            await UserManager.CreateAsync(user, "Password123");

            var createdUser = await UserManager.FindByIdAsync(user.Id.ToString());

            foreach (var role in roles)
            {

                await UserManager.AddToRoleAsync(createdUser, role);

            }

            var token = await _tokenService.CreateToken(createdUser);

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
