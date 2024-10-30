using System.Security.Claims;
using FirebaseAdmin.Auth;
using Shield.Api.Common.Abstractions;
using Shield.Api.Common.Exceptions;
using Shield.Api.Configurations;

namespace Shield.Api.Infrastructure.Identity;

internal sealed class IdentityService : IIdentityService
{
    private readonly string _userEmailDomain;
    private readonly FirebaseAuth _firebaseAuth;

    public IdentityService(Settings settings, FirebaseAuth firebaseAuth = null)
    {
        _userEmailDomain = settings.Firebase.UserEmailDomain;
        _firebaseAuth = firebaseAuth ?? FirebaseAuth.DefaultInstance;
    }

    public async Task<string> SignUpMemberAsync(string username, string password, List<string> roles = null)
    {
        try
        {
            var email = $"{username}@{_userEmailDomain}";
        
            var userArgs = new UserRecordArgs
            {
                Email = email,
                Password = password,
                DisplayName = username
            };
            
            var userRecord = await _firebaseAuth.CreateUserAsync(userArgs);
            
            var customClaims = new Dictionary<string, object>
            {
                { ClaimTypes.NameIdentifier, userRecord.Uid },
                { ClaimTypes.Name, username },
                { ClaimTypes.Role, roles }
            };
            
            await _firebaseAuth.SetCustomUserClaimsAsync(userRecord.Uid, customClaims);
            
            return userRecord.Uid;
        }
        catch (FirebaseAuthException ex) when (ex.AuthErrorCode == AuthErrorCode.EmailAlreadyExists)
        {
            throw new ConflictException("Username already exists");
        }
        catch (Exception)
        {
            throw new ServerErrorException("An unexpected error occurred during sign-up");
        }
    }
}
