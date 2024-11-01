using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Social.Application.Interfaces;
using Social.Infrastructure.Data;
using Social.Infrastructure.Services;

namespace Social.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<ISocialDbContext>(provider => provider.GetService<DataContext>());
        
        var cloudinarySettings = configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
        var account = new Account(
            cloudinarySettings.CloudName,
            cloudinarySettings.ApiKey,
            cloudinarySettings.ApiSecret
        );
        var cloudinary = new Cloudinary(account) { Api = { Secure = true } };
        services.AddSingleton(cloudinary);
        
        services.AddScoped<IPhotoService, PhotoService>();

        return services;
    }
}
