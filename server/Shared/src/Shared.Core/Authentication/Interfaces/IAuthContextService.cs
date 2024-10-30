namespace Shared.Core.Authentication.Interfaces;

public interface IAuthContextService
{
    string? GetUserId();
    string? GetUsername();
    IEnumerable<string> GetUserRoles();
}