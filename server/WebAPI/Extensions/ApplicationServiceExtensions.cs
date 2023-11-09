/*using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Behaviors;
using WebAPI.Data;
using WebAPI.Interfaces;
using WebAPI.Services;
using System.Reflection;

namespace WebAPI.Extensions
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
                *//*cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));*//*
            });

            return services;
        }
    }
}*/
