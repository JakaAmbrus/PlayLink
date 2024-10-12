namespace Shield.Api.Common.Abstractions;

public interface IIdentityService
{
    Task<string> SignUpMemberAsync(string username, string password);
}