using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPI.Filters
{
    public class SwaggerAuthorizationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo.GetCustomAttributes(true)
                 .Union(context.MethodInfo.DeclaringType.GetCustomAttributes(true))
                 .OfType<AuthorizeAttribute>()
                 .Distinct();
            
             if (authAttributes.Any())
             {
                 operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                 operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
            
                 operation.Security = new List<OpenApiSecurityRequirement>
                 {
                     new OpenApiSecurityRequirement
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
                             authAttributes.Select(attr => attr.Policy).ToArray()
                       }
                     }
                 };
             }
        }
    }
}
