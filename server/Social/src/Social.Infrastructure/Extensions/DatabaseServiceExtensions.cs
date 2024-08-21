using Social.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Social.Infrastructure.Data;
using Social.Infrastructure.Services;

namespace Social.Infrastructure.Extensions
{
    public static class DatabaseServiceExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            }); 
 
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<DataContext>());

            services.AddScoped<IUserManager, UserManagerService>();

            services.AddCors();

            return services;
        }
    }
}
