using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using server.Behaviors;
using server.Data;
using server.Interfaces;
using server.Services;
using System.Reflection;

namespace server.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                /*cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));*/
            });

            return services;
        }
    }
}
