using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Social.Application.Behaviors;
using Social.Application.Interfaces;
using Social.Application.Services;

namespace Social.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(configuration => 
                configuration.RegisterServicesFromAssembly(assembly));
            services.AddValidatorsFromAssembly(assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddMemoryCache();
            services.AddScoped<ICacheKeyService, CacheKeyService>();
            services.AddScoped<ICacheInvalidationService, CacheInvalidationService>();

            return services;
        }
    }
}
