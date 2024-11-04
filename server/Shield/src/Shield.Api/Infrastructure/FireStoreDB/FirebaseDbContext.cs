using Google.Cloud.Firestore;
using Shared.Core.Enums;
using Shared.Core.Security;
using Shield.Api.Common.Abstractions;
using Shield.Api.Common.Exceptions;
using Shield.Api.Configurations;

namespace Shield.Api.Infrastructure.FireStoreDB;

public class FirebaseDbContext : IFirebaseDbContext
{
    private readonly FirestoreDb _firestoreDb;
    private readonly FirestoreOptions _options;

    public FirebaseDbContext(FirestoreDb firestoreDb, Settings settings)
    {
        _firestoreDb = firestoreDb;
        _options = settings.Firebase.Firestore;
    }

    public async Task<DocumentSnapshot> GetUserByIdAsync(string userId)
    {
        try
        {
            var docRef = GetUserDocumentReference(userId);
            var snapshot = await docRef.GetSnapshotAsync();
            
            if (!snapshot.Exists)
            {
                throw new NotFoundException("User not found.");
            }
            
            return snapshot;
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

    public async Task AddRolesAsync(string userId, List<string> rolesToAdd)
    {
        try
        {
            var docRef = GetUserDocumentReference(userId);
            var snapshot = await docRef.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                throw new NotFoundException("User not found.");
            }

            var data = snapshot.ToDictionary();
            var rolesFieldName = _options.Fields.RolesField;

            if (data.TryGetValue(rolesFieldName, out var value))
            {
                if (value is List<object> existingRoles)
                {
                    var rolesSet = new HashSet<string>(existingRoles.Select(r => r.ToString()));
                    rolesToAdd.ForEach(role => rolesSet.Add(role));
                    data[rolesFieldName] = rolesSet.ToList();
                }
            }
            else
            {
                data[rolesFieldName] = rolesToAdd;
            }

            await docRef.SetAsync(data, SetOptions.MergeAll);
        }
        catch (Exception)
        {
            throw new ServerErrorException("An unexpected error occurred while adding roles.");
        }
    }

    public async Task RemoveRoleAsync(string userId, string roleToRemove)
    {
        try
        {
            var docRef = GetUserDocumentReference(userId);
            var snapshot = await docRef.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                throw new NotFoundException("User not found.");
            }

            var data = snapshot.ToDictionary();
            var rolesFieldName = _options.Fields.RolesField;

            if (data.TryGetValue(rolesFieldName, out var value))
            {
                if (value is List<object> roles)
                {
                    var rolesSet = new HashSet<string>(roles.Select(r => r.ToString()));
                    if (!rolesSet.Remove(roleToRemove))
                    {
                        throw new BadRequestException($"Role '{roleToRemove}' not found for user.");
                    }

                    data[rolesFieldName] = rolesSet.ToList();
                    await docRef.SetAsync(data, SetOptions.MergeFields(rolesFieldName));
                }
                else
                {
                    throw new BadRequestException($"No roles found for user.");
                }
            }
        }
        catch (Exception)
        {
            throw new ServerErrorException("An unexpected error occurred while removing the role.");
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
