using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shield.Api.Configurations;
using Shield.Api.Features.SignUp;
using Shield.Api.Middleware;

namespace Shield.Api;

public class Startup
{
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // Options pattern setup
        var settings = configuration.Get<Settings>();
        services.AddSingleton<Settings>(settings);

        // JWT Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://login.microsoftonline.com/{tenant-id}";
                options.Audience = "{your-client-id}";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true
                };
            });
        
        // Authorization
        services.AddAuthorizationBuilder()
            .AddPolicy("MemberPolicy", policy => policy.RequireRole("Member"))
            .AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
        
        // Mediatr pipeline
        var assembly = typeof(Startup).Assembly;
        services.AddMediatR(x => x.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        
        // Endpoints
        app.UseEndpoints(endpoint =>
        {
            endpoint.MapSignUpEndpoint();
        });
    }
}
