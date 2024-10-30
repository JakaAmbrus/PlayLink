using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Authentication.Interfaces;
using Shared.Core.Authentication.Services;

namespace Shared.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedSecurity(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IAuthContextService, AuthContextService>();
        return services;
    }
}