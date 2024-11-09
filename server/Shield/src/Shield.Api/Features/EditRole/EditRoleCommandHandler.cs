using MediatR;
using Shared.Core.Security;
using Shield.Api.Common.Abstractions;
using Shield.Api.Common.Exceptions;

namespace Shield.Api.Features.EditRole;

public class EditRoleCommandHandler : IRequestHandler<EditRoleCommand, EditRoleResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IFirestoreDbContext _firestoreDbContext;

    public EditRoleCommandHandler(IIdentityService identityService, IFirestoreDbContext firestoreDbContext)
    {
        _identityService = identityService;
        _firestoreDbContext = firestoreDbContext;
    }

    public async Task<EditRoleResponse> Handle(EditRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _firestoreDbContext.GetUserByIdAsync(request.UserId)
            ?? throw new NotFoundException("User not found");

        var roles = user.Roles;

        if (roles.Contains(Roles.Moderator))
        {
            roles.Remove(Roles.Moderator);
        }
        else
        {
            roles.Add(Roles.Moderator);
        }

        await _firestoreDbContext.UpdateUserRolesAsync(user.UserId, roles);
        await _identityService.SetUserClaimsAsync(user.UserId, user.Username, user.SocialId, roles);
        return new EditRoleResponse();
    }
}