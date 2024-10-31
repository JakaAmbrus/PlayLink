using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
var configDirectory = Path.Combine(Directory.GetCurrentDirectory(), env.EnvironmentName);
builder.Configuration.AddOcelot(configDirectory, builder.Environment);

builder.Services.AddOcelot();

builder.Services.AddEndpointsApiExplorer();

var firebaseProjectId = builder.Configuration["FirebaseProjectId"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("JwtBearer", options =>
    {
        options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
            ValidateAudience = true,
            ValidAudience = $"{firebaseProjectId}",
            ValidateLifetime = true,
            RoleClaimType = ClaimTypes.Role
        };
    });

var app = builder.Build();

if (env.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseOcelot().Wait();

app.Run();