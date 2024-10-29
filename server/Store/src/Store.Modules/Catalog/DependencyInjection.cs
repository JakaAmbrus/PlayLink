using System.Reflection;
using Catalog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogModuleServices(this IServiceCollection services, IConfiguration configuration, List<Assembly> mediatrAssemblies)
    {
        // Settings setup
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        var settings = configuration.GetSection(assemblyName!).Get<Settings>();
        services.AddSingleton(settings!);
        
        // Mediator pipeline setup
        mediatrAssemblies.Add(typeof(DependencyInjection).Assembly);
        
        // Database setup
        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseSqlServer(settings!.ConnectionString);
        });
        
        return services;
    }
}