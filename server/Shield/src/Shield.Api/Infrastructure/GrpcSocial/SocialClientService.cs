using Grpc.Core;
using Shared.Grpc;
using Shield.Api.Common.Abstractions;

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
        catch (RpcException ex)
        {
            // Handle gRPC exceptions, log errors, or rethrow as necessary
            Console.WriteLine($"Error calling RegisterUser: {ex.Status.Detail}");
            throw;
        }
    }
}