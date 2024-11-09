using MediatR;

namespace Shield.Api.Features.EditRole;

public class EditRoleCommand : IRequest<EditRoleResponse>
{
    public string UserId { get; set; }
}