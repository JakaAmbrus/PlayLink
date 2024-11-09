using MediatR;
using Shield.Api.Common.Abstractions;

namespace Shield.Api.Features.DeleteAccount;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, DeleteAccountResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IFirestoreDbContext _firestoreDbContext;
    private readonly ISocialClientService _socialClientService;

    public DeleteAccountCommandHandler(IIdentityService identityService, IFirestoreDbContext firestoreDbContext, ISocialClientService socialClientService)
    {
        _identityService = identityService;
        _firestoreDbContext = firestoreDbContext;
        _socialClientService = socialClientService;
    }

    public async Task<DeleteAccountResponse> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        await _identityService.DeleteUserAsync(request.UserId);
        await _firestoreDbContext.DeleteUserAsync(request.UserId);
        await _socialClientService.DeleteUserAsync(request.SocialId, false);
        return new DeleteAccountResponse();
    }
}