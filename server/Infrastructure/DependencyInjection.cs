﻿using Infrastructure.Extensions;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseServices(configuration);

        services.AddIdentityServices(configuration);
        services.AddCloudinaryServices(configuration);

        services.AddTokenServices();

        services.AddAuthenticatedUserServices();

        services.AddSingleton<ICountryService, CountryService>();

        services.AddScoped<IUserActivityService, UserActivityService>();

        return services;
    }
}
