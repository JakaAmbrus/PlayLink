using MediatR;
using Shield.Api.Common.Abstractions;

namespace Shield.Api.Features.SignUp;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignUpResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IFirebaseDbContext _firebaseDbContext;

    public SignUpCommandHandler(IIdentityService identityService, IFirebaseDbContext firebaseDbContext)
    {
        _identityService = identityService;
        _firebaseDbContext = firebaseDbContext;
    }

    public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var userId = await _identityService.SignUpMemberAsync(request.Username, request.Password);
        
        // Now I go into Social and create the user, smt like:
        // await _socialApi.AddUserAsync(request) 
        // RabbitMq notifies store to make a coupon for the new member 20% discount
        // Todo: also do not forget to implement rollback if any of these fails
        const long socialId = 1;
        await _firebaseDbContext.AddUserAsync(userId, request.Username, socialId);

        return new SignUpResponse();
    }
}