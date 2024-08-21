using Microsoft.Extensions.DependencyInjection;
using Social.Application.Interfaces;
using Social.Application.Services;

namespace Social.Application.Extensions
{
    public static class AuthenticatedUserServiceExtensions
    {
        public static IServiceCollection AddAuthenticatedUserServices(this IServiceCollection services)
        {

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
