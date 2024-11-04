using Microsoft.AspNetCore.Authorization;

namespace Shared.Core.Security;

public class ForbidRoleHandler : AuthorizationHandler<ForbidRoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ForbidRoleRequirement requirement)
    {
        if (context.User.IsInRole(requirement.ForbiddenRole))
        {
            context.Fail();
        }
        else
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}