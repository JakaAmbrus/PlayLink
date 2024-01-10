using Application.Interfaces;
using CloudinaryDotNet;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class CloudinaryServiceExtensions
    {
        public static IServiceCollection AddCloudinaryServices(this IServiceCollection services)
        {
            var cloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME") ?? "AccountName";
            var apiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY") ?? "";
            var apiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET") ?? "";

            var account = new Account(cloudName, apiKey, apiSecret);

            var cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;

            services.AddSingleton(cloudinary);
            services.AddScoped<IPhotoService, PhotoService>();

            return services;
        }
    }
}