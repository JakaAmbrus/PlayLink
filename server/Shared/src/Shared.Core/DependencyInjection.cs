using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Core.Authentication.Interfaces;
using Shared.Core.Authentication.Services;

namespace Shared.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedSecurity(this IServiceCollection services, string firebaseProjectId)
    {
        services.AddHttpContextAccessor();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
                    ValidateAudience = true,
                    ValidAudience = $"{firebaseProjectId}",
                    ValidateLifetime = true
                };
            });
        services.AddScoped<IAuthContextService, AuthContextService>();
        return services;
    }
}