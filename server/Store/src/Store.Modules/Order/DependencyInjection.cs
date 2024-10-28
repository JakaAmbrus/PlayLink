using System.Reflection;
using Discount.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Shared.Behaviours;

namespace Order;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderModuleServices(this IServiceCollection services, IConfiguration configuration, List<Assembly> mediatrAssemblies)
    {
        // Settings setup
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        var settings = configuration.GetSection(assemblyName!).Get<Settings>();
        services.AddSingleton(settings!);
        
        // Mediator pipeline setup
        mediatrAssemblies.Add(typeof(DependencyInjection).Assembly);
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        // Database setup
        services.AddDbContext<OrderDbContext>(options =>
        {
            options.UseSqlServer(settings!.ConnectionString);
        });
        
        return services;
    }
}