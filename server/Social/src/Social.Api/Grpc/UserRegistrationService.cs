using Grpc.Core;
using MediatR;
using Shared.Grpc;
using Social.Application.Features.Authentication.UserRegistration;
using Social.Domain.Exceptions;

namespace Social.Api.Grpc;

public class UserRegistrationService : UserRegistration.UserRegistrationBase
{
    private readonly ISender _mediator;

    public UserRegistrationService(ISender mediator)
    {
        _mediator = mediator;
    }

    public override async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request, ServerCallContext context)
    {
        try
        {
            if (!DateOnly.TryParse(request.DateOfBirth, out var dateOfBirth))
            {
                throw new ArgumentException("Invalid date format for DateOfBirth.");
            }

            var command = new UserRegistrationCommand
            {
                Username = request.Username,
                Country = request.Country,
                FullName = request.Fullname,
                Gender = request.Gender,
                DateOfBirth = dateOfBirth,
            };
            
            var result = await _mediator.Send(command);

            return new RegisterUserResponse
            {
                Success = true,
                ErrorMessage = "",
                ErrorCode = "",
                SocialId = result.SocialId,
            };
        }
        catch (ApplicationExceptions ex)
        {
            return new RegisterUserResponse
            {
                Success = false,
                ErrorMessage = ex.Message,
                ErrorCode = ex.StatusCode.ToString(),
                SocialId = 0,
            };
        }
        catch (Exception)
        {
            return new RegisterUserResponse
            {
                Success = false,
                ErrorMessage = "An unexpected error occurred.",
                ErrorCode = "500",
                SocialId = 0,
            };
        }
    }
}