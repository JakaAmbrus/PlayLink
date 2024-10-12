using System.Security.Claims;
using FirebaseAdmin.Auth;
using Shield.Api.Common.Abstractions;

namespace Shield.Api.Infrastructure.Identity;

internal sealed class IdentityService : IIdentityService
{
    public async Task<string> SignUpMemberAsync(string username, string password, List<string> roles)
    {
        try
        {
            var email = $"{username}@playlink.com";
        
            var userArgs = new UserRecordArgs
            {
                Email = email,
                Password = password,
                DisplayName = username
            };

            var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

            var customClaims = new Dictionary<string, object>
            {
                { ClaimTypes.NameIdentifier, userRecord.Uid },
                { ClaimTypes.Name, username },
                { ClaimTypes.Role, roles }
            };
        
            await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(userRecord.Uid, customClaims);

            return userRecord.Uid;
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
        }
    }
}