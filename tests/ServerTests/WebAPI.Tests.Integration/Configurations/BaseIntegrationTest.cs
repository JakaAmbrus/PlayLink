﻿using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace WebAPI.Tests.Integration.Configurations
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
    {
        private readonly IServiceScope _scope;
        protected readonly ISender Mediator;
        protected readonly DataContext Context;
        protected readonly IUserManager UserManager;
        protected readonly HttpClient Client;
        private readonly ITokenService _tokenService;
        protected readonly IPhotoService PhotoService;

        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();
            Mediator = _scope.ServiceProvider.GetRequiredService<ISender>();
            Context = _scope.ServiceProvider.GetRequiredService<DataContext>();
            UserManager = _scope.ServiceProvider.GetRequiredService<IUserManager>();
            _tokenService = _scope.ServiceProvider.GetRequiredService<ITokenService>();
            Client = factory.CreateClient();
            PhotoService = _scope.ServiceProvider.GetRequiredService<IPhotoService>();
        }

        protected async Task InitializeAuthenticatedClient()
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

            UserManager.CreateAsync(user, "Password123").Wait();

            var createdUser = await UserManager.FindByIdAsync(user.Id.ToString());

            await UserManager.AddToRoleAsync(createdUser, "Member");

            var token = await _tokenService.CreateToken(createdUser);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
