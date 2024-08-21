using Microsoft.Extensions.DependencyInjection;
using Social.Application.Interfaces;
using Social.Application.Services;

namespace Social.Application.Extensions
{
    public static class TokenServiceExtensions
    {
        public static IServiceCollection AddTokenServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
