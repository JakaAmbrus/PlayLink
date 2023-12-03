using Application.Interfaces;
using CloudinaryDotNet;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class CloudinaryServiceExtensions
    {
        public static IServiceCollection AddCloudinaryServices(this IServiceCollection services, IConfiguration config)
        {
            var cloudinarySettings = config.GetSection("CloudinarySettings").Get<CloudinarySettings>();
            var account = new Account(
                cloudinarySettings.CloudName,
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret);

            var cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;

            services.AddSingleton(cloudinary);
            services.AddScoped<IPhotoService, PhotoService>();

            return services;
        }
    }
}
