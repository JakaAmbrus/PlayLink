using MediatR;
using Shield.Api.Common.Abstractions;
using Shield.Api.Common.Exceptions;

namespace Shield.Api.Features.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IFirestoreDbContext _firestoreDbContext;
    private readonly ISocialClientService _socialClientService;

    public DeleteUserCommandHandler(IIdentityService identityService, IFirestoreDbContext firestoreDbContext, ISocialClientService socialClientService)
    {
        _identityService = identityService;
        _firestoreDbContext = firestoreDbContext;
        _socialClientService = socialClientService;
    }

    public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _firestoreDbContext.GetUserByIdAsync(request.UserId)
            ?? throw new NotFoundException("User not found");
        
        await _identityService.DeleteUserAsync(user.UserId);
        await _firestoreDbContext.DeleteUserAsync(user.UserId);
        await _socialClientService.DeleteUserAsync(user.SocialId, false);
        
        return new DeleteUserResponse();
    }
}