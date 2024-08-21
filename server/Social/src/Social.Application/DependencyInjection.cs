using Social.Application.Extensions;
using Social.Application.Interfaces;
using Social.Application.Services;
using FluentValidation;

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Social.Application.Behaviors;

namespace Social.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(configuration => 
                configuration.RegisterServicesFromAssembly(assembly));

            services.AddValidatorsFromAssembly(assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddTokenServices();

            services.AddAuthenticatedUserServices();

            services.AddMemoryCacheExtensions();

            return services;
        }
    }
}
