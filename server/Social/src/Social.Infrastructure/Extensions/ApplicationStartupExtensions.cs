using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Social.Infrastructure.Data;

namespace Social.Infrastructure.Extensions
{
    public static class ApplicationStartupExtensions
    {
        public static void ConfigureApplication(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;
                Seed.SeedData(scopedProvider).Wait();
            }

            app.UseHttpsRedirection();

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                var corsOrigins = new List<string>
                {
                    "https://ambrusocialmedia.com",
                    "https://www.ambrusocialmedia.com"
                };

                // this allows tesing in production mode with docker compose, wrote like this to emphasize
                if (Environment.GetEnvironmentVariable("ALLOW_DEV_ORIGIN") == "true")
                {
                    corsOrigins.Add("http://localhost:4200");
                }

                app.UseCors(builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins(corsOrigins.ToArray()));
            }
            else
            {
                // Development
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedProvider = scope.ServiceProvider;
                    Seed.SeedData(scopedProvider).Wait();
                }

                app.UseCors(builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("https://localhost:4200", "http://localhost:4200"));
            }

            app.UseIpRateLimiting();
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
