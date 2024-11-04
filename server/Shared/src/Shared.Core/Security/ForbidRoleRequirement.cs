using Microsoft.AspNetCore.Authorization;

namespace Shared.Core.Security;

public class ForbidRoleRequirement : IAuthorizationRequirement
{
    public string ForbiddenRole { get; set; }
    
    public ForbidRoleRequirement(string role)
    {
        ForbiddenRole = role;
    }
}