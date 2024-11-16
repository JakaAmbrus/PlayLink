using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Basket;

public static class DependencyInjection
{
    public static IServiceCollection AddBasketModuleServices(this IServiceCollection services, IConfiguration configuration, List<Assembly> mediatrAssemblies)
    {
        // Settings setup
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        var settings = configuration.GetSection(assemblyName!).Get<Settings>();
        services.AddSingleton(settings!);
        
        // Mediator pipeline setup
        mediatrAssemblies.Add(typeof(DependencyInjection).Assembly);
        
        // Redis DB setup
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(settings.ConnectionString));
        
        return services;
    }
}