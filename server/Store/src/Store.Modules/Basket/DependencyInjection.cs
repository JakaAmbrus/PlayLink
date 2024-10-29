using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Store.Shared.Behaviours;

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
        
        return services;
    }
}