namespace Shield.Api.Common.Abstractions;

public interface IIdentityService
{
    Task<string> SignUpMemberAsync(string username, string password);

    Task SetUserClaimsAsync(string userId, string socialId, string username, List<string> roles);
}