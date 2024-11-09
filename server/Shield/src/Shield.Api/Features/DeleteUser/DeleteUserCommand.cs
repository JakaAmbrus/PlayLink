using MediatR;

namespace Shield.Api.Features.DeleteUser;

public class DeleteUserCommand : IRequest<DeleteUserResponse>
{
    public string UserId { get; set; }
}