using System.Security.Claims;
using System.Text;
using System.Text.Json;
using FirebaseAdmin.Auth;
using Shield.Api.Common.Abstractions;
using Shield.Api.Common.Configurations;
using Shield.Api.Common.Exceptions;

namespace Shield.Api.Infrastructure.Identity;

internal sealed class IdentityService : IIdentityService
{
    private readonly FirebaseOptions _firebaseSettings;
    private readonly FirebaseAuth _firebaseAuth;
    private static readonly HttpClient HttpClient = new HttpClient();

    public IdentityService(Settings settings, FirebaseAuth firebaseAuth = null)
    {
        _firebaseSettings = settings.Firebase;
        _firebaseAuth = firebaseAuth ?? FirebaseAuth.DefaultInstance;
    }

    public async Task<string> SignInAsync(string email, string password)
    {
        var requestBody = new
        {
            email = email,
            password = password,
            returnSecureToken = true
        };
        
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        
        var response = await HttpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_firebaseSettings.ApiKey}", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            throw new ServerErrorException("Sign in failed.");
        }
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseData = JsonSerializer.Deserialize<SignInResponse>(responseContent);

        return responseData.IdToken;
    }

    public async Task<string> SignUpMemberAsync(string username, string password)
    {
        try
        {
            var email = $"{username}@{_firebaseSettings.UserEmailDomain}";
        
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

    public async Task SetUserClaimsAsync(string userId, string username, int socialId, List<string> roles)
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
    
    public async Task DeleteUserAsync(string userId)
    {
        try
        {
            await _firebaseAuth.DeleteUserAsync(userId);
        }
        catch (FirebaseAuthException ex) when (ex.AuthErrorCode == AuthErrorCode.UserNotFound)
        {
            throw new NotFoundException("User not found");
        }
        catch (Exception)
        {
            throw new ServerErrorException("An unexpected error occurred while deleting the user");
        }
    }
    
    private class SignInResponse
    {
        public string IdToken { get; set; }
    }
}
