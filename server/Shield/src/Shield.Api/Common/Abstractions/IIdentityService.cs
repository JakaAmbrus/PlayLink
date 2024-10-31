namespace Shield.Api.Common.Abstractions;

public interface IIdentityService
{
    Task<string> SignUpMemberAsync(string username, string password);

    Task SetUserClaimsAsync(string userId, string username, long socialId, List<string> roles);
}