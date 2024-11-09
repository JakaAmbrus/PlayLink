using Google.Cloud.Firestore;
using Shared.Core.Security;
using Shield.Api.Common.Abstractions;
using Shield.Api.Common.Configurations;
using Shield.Api.Common.Exceptions;
using Shield.Api.Common.Models;

namespace Shield.Api.Infrastructure.FireStoreDB;

public class FirestoreDbContext : IFirestoreDbContext
{
    private readonly FirestoreDb _firestoreDb;
    private readonly FirestoreOptions _options;

    public FirestoreDbContext(FirestoreDb firestoreDb, Settings settings)
    {
        _firestoreDb = firestoreDb;
        _options = settings.Firebase.Firestore;
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        try
        {
            var docRef = GetUserDocumentReference(userId);
            var snapshot = await docRef.GetSnapshotAsync();
        
            if (!snapshot.Exists)
            {
                return null;
            }

            var user = new User
            {
                UserId = snapshot.GetValue<string>(_options.Fields.UserId),
                Username = snapshot.GetValue<string>(_options.Fields.Username),
                SocialId = snapshot.GetValue<int>(_options.Fields.SocialId),
                Roles = snapshot.GetValue<List<string>>(_options.Fields.RolesField) ?? new List<string>()
            };
        
            return user;
        }
        catch (Exception)
        {
            throw new ServerErrorException("An unexpected error occurred while retrieving the user.");
        }
    }

    public async Task AddUserAsync(string userId, string username, long socialId)
    {
        try
        {
            var initialRoles = new List<string> { Roles.Member };
            var userData = new Dictionary<string, object>
            {
                { _options.Fields.UserId, userId },
                { _options.Fields.SocialId, socialId },
                { _options.Fields.Username, username },
                { _options.Fields.RolesField, initialRoles }
            };
            
            var docRef = GetUserDocumentReference(userId);
            await docRef.SetAsync(userData, SetOptions.MergeAll);
        }
        catch (Exception)
        {
            throw new ServerErrorException("An unexpected error occurred while adding the user.");
        }
    }

    public async Task UpdateUserRolesAsync(string userId, List<string> roles)
    {
        try
        {
            var docRef = GetUserDocumentReference(userId);
            
            var updates = new Dictionary<string, object>
            {
                { _options.Fields.RolesField, roles }
            };
            
            await docRef.UpdateAsync(updates);
        }
        catch (Exception)
        {
            throw new ServerErrorException("An unexpected error occurred while updating the user.");
        }
    }

    public async Task DeleteUserAsync(string userId)
    {
        try
        {
            var docRef = GetUserDocumentReference(userId);
            await docRef.DeleteAsync();
        }
        catch (Exception)
        {
            throw new ServerErrorException("An unexpected error occurred while deleting the user.");
        }
    }

    private DocumentReference GetUserDocumentReference(string userId)
    {
        return _firestoreDb.Collection(_options.Collection).Document(userId);
    }
}
