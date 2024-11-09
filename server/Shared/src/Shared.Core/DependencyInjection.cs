using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Core.Security;

namespace Shared.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedSecurity(this IServiceCollection services, string firebaseProjectId)
    {
        services.AddHttpContextAccessor();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
                    ValidateAudience = true,
                    ValidAudience = firebaseProjectId,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.StatusCode = 498; // For client logout, I have my reasons
                        context.Response.ContentType = "application/json";

                        const string errorMessage = "Invalid or expired token";
                        var result = JsonSerializer.Serialize(new { message = errorMessage });
                        return context.Response.WriteAsync(result);
                    }
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("Admin", policy => policy.RequireRole(Roles.Admin))
            .AddPolicy("Moderator", policy => policy.RequireRole(Roles.Moderator))
            .AddPolicy("Member", policy => policy.RequireRole(Roles.Member))
            .AddPolicy("DenyGuestRole", policy => policy.Requirements.Add(new ForbidRoleRequirement(Roles.Guest)));
        
        services.AddAuthorization();
        
        services.AddScoped<IAuthContextService, AuthContextService>();
        services.AddSingleton<IAuthorizationHandler, ForbidRoleHandler>();
        
        return services;
    }
}