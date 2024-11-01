namespace Shared.Core.Authentication.Interfaces;

public interface IAuthContextService
{
    string? GetUserId();
    string? GetUsername();
    int GetSocialId();
    IEnumerable<string> GetUserRoles();
}