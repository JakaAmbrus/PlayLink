using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class AuthenticatedUserServiceExtensions
    {
        public static IServiceCollection AddAuthenticatedUserServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();

            return services;
        }
    }
}
