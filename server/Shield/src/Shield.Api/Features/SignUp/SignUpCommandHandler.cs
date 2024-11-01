using MediatR;
using Microsoft.IdentityModel.Tokens;
using Shared.Core.Enums;
using Shield.Api.Common.Abstractions;

namespace Shield.Api.Features.SignUp;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignUpResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IFirebaseDbContext _firebaseDbContext;
    private readonly ISocialClientService _socialClientService;

    public SignUpCommandHandler(IIdentityService identityService, IFirebaseDbContext firebaseDbContext, ISocialClientService socialClientService)
    {
        _identityService = identityService;
        _firebaseDbContext = firebaseDbContext;
        _socialClientService = socialClientService;
    }

    public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        // var userId = await _identityService.SignUpMemberAsync(request.Username, request.Password);

        var socialResponse = await _socialClientService
            .RegisterUserAsync(
                request.Username, 
                request.Gender,
                request.FullName, 
                request.Country, 
                request.DateOfBirth);

        int socialId = socialResponse.SocialId;
        
        if (!socialResponse.ErrorMessage.IsNullOrEmpty() || socialId == 0)
        {
            // ERROR
        }
        
        // await _firebaseDbContext.AddUserAsync(userId, request.Username, socialId);
        
        // await _identityService.SetUserClaimsAsync(userId , request.Username, socialId, [Role.Member.ToString()]);
        
        // RabbitMq notifies store to make a coupon for the new member 20% discount
        // Todo: also do not forget to implement rollback if any of these fails

        return new SignUpResponse();
    }
}