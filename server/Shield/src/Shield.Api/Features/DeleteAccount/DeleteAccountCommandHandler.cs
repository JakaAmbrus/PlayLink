using MediatR;
using Shield.Api.Common.Abstractions;

namespace Shield.Api.Features.DeleteAccount;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, DeleteAccountResponse>
{
    private readonly IIdentityService _identityService;

    public DeleteAccountCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<DeleteAccountResponse> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        return new DeleteAccountResponse { Deleted = true };
    }
}