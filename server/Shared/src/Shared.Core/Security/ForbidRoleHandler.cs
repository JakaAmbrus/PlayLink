using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Shared.Core.Security;

public class ForbidRoleHandler : AuthorizationHandler<ForbidRoleRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ForbidRoleHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ForbidRoleRequirement requirement)
    {
        if (context.User.IsInRole(requirement.ForbiddenRole))
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                httpContext.Response.StatusCode = 403;
                httpContext.Response.ContentType = "application/json";
                var message = JsonSerializer.Serialize(new { message = "Access denied for Guest role." });
                httpContext.Response.WriteAsync(message).Wait();
            }
            
            context.Fail();
        }
        else
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}