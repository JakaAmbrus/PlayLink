using Microsoft.OpenApi.Models;
using System.Reflection;
using WebAPI.Filters;

namespace WebAPI.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PlayLink API",
                    Version = "v1",
                    Description = "The PlayLink API is the backbone of my social media platform. " +
                    "To test PlayLink endpoints in Swagger, " +
                    "I suggest you use Guest Login to receive a valid role JWT." +
                    "Then put 'Bearer (paste the token)' in the Authorize section." +
                    "In order to test the Admin endpoints, please refer to the documentation."
                });

                // JWT Bearer Authorization
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                c.OperationFilter<SwaggerAuthorizationFilter>(); // Excludes Account Controller endpoints from JWT Authorization

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}
