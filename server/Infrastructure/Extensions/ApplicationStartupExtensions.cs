using AspNetCoreRateLimit;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
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
                // Production
                app.UseCors(builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("??"));

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
                    .WithOrigins("https://localhost:4200"));
            }

            app.UseHttpsRedirection();
            app.UseIpRateLimiting();
            app.UseAuthentication();
            app.UseAuthorization();

        }
    }
}
