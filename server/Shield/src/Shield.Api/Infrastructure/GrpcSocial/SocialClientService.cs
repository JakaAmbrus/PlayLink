using Grpc.Core;
using Shared.Grpc;
using Shield.Api.Common.Abstractions;
using Shield.Api.Common.Exceptions;

namespace Shield.Api.Infrastructure.GrpcSocial;

public class SocialClientService : ISocialClientService
{
    private readonly UserRegistration.UserRegistrationClient _client;

    public SocialClientService(UserRegistration.UserRegistrationClient client)
    {
        _client = client;
    }

    public async Task<RegisterUserResponse> RegisterUserAsync(string username, string gender, string fullname, string country, DateTime dateOfBirth)
    {
        try
        {
            var request = new RegisterUserRequest
            {
                Username = username,
                Gender = gender,
                Fullname = fullname,
                Country = country,
                DateOfBirth = DateOnly.FromDateTime(dateOfBirth).ToString(),
            };

            return await _client.RegisterUserAsync(request);
        }
        catch (RpcException)
        {
            throw new ServerErrorException("Error occured while registering user");
        }
    }
    
    public async Task<DeleteUserResponse> DeleteUserAsync(int socialId)
    {
        try
        {
            var request = new DeleteUserRequest
            {
                SocialId = socialId,
            };

            return await _client.DeleteUserAsync(request);
        }
        catch (RpcException)
        {
            throw new ServerErrorException("Error occured while registering user");
        }
    }
}