using Shared.Grpc;

namespace Shield.Api.Common.Abstractions;

public interface ISocialClientService
{
    Task<RegisterUserResponse> RegisterUserAsync(string username, string gender, string fullname, string country, DateTime dateOfBirth, string userId);

    Task<DeleteUserResponse> DeleteUserAsync(int socialId);
}