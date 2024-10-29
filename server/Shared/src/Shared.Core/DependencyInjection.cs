using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Interfaces;
using Shared.Core.Services;

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