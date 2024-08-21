using Microsoft.Extensions.DependencyInjection;
using Social.Application.Interfaces;
using Social.Application.Services;

namespace Social.Application.Extensions
{
    public static class MemoryCacheExtensions
    {
        public static IServiceCollection AddMemoryCacheExtensions(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddScoped<ICacheKeyService, CacheKeyService>();
            services.AddScoped<ICacheInvalidationService, CacheInvalidationService>();

            return services;
        }
    }
}
