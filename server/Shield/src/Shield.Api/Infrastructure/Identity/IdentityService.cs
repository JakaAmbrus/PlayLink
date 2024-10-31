using System.Security.Claims;
using FirebaseAdmin.Auth;
using Shared.Core.Enums;
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

    public async Task<string> SignUpMemberAsync(string username, string password)
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

    public async Task SetUserClaimsAsync(string userId, string username, long socialId, List<string> roles)
    {
        try
        {
            var customClaims = new Dictionary<string, object>
            {
                { ClaimTypes.NameIdentifier, userId },
                { ClaimTypes.PrimarySid, socialId },
                { ClaimTypes.Name, username },
                { ClaimTypes.Role, roles }
            };

            await _firebaseAuth.SetCustomUserClaimsAsync(userId, customClaims);
        }
        catch
        {
            throw new ServerErrorException("An unexpected error occurred during claims assignment");
        }
    }
}
