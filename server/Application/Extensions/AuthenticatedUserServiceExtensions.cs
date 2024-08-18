using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
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
