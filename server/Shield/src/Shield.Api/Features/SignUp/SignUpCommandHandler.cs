using MediatR;
using Shared.Core.Security;
using Shield.Api.Common.Abstractions;
using Shield.Api.Common.Exceptions;

namespace Shield.Api.Features.SignUp;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignUpResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IFirestoreDbContext _firestoreDbContext;
    private readonly ISocialClientService _socialClientService;

    public SignUpCommandHandler(IIdentityService identityService, IFirestoreDbContext firestoreDbContext, ISocialClientService socialClientService)
    {
        _identityService = identityService;
        _firestoreDbContext = firestoreDbContext;
        _socialClientService = socialClientService;
    }

    public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        string userId = null;
        int socialId = 0;

        try
        {
            userId = await _identityService.SignUpMemberAsync(request.Username, request.Password);

            var socialResponse = await _socialClientService.RegisterUserAsync(
                request.Username,
                request.Gender,
                request.FullName,
                request.Country,
                request.DateOfBirth,
                userId);

            if (!string.IsNullOrEmpty(socialResponse.ErrorMessage) || socialResponse.SocialId == 0)
            {
                throw new ServerErrorException("Social registration failed");
            }

            socialId = socialResponse.SocialId;

            await _firestoreDbContext.AddUserAsync(userId, request.Username, socialId);

            await _identityService.SetUserClaimsAsync(userId, request.Username, socialId, [Roles.Member]);

            // Todo: rabbitmq message for discount on store service
        }
        catch (ConflictException)
        {
            throw;
        }
        catch (Exception)
        {
            if (userId != null)
            {
                await _identityService.DeleteUserAsync(userId);
            }
            if (socialId != 0)
            {
                await _socialClientService.DeleteUserAsync(socialId, true);
            }

            throw;
        }

        return new SignUpResponse { };
    }
}