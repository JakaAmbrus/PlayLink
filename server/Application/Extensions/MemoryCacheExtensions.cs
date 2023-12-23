using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class MemoryCacheExtensions
    {
        public static IServiceCollection AddMemoryCacheExtensions(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddScoped<ICacheInvalidationService, CacheInvalidationService>();

            return services;
        }
    }
}
