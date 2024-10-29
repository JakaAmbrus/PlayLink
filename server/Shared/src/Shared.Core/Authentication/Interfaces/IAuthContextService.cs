namespace Shared.Core.Interfaces;

public interface IAuthContextService
{
    string? GetUserId();
    string? GetUsername();
    IEnumerable<string> GetUserRoles();
}