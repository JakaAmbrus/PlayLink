using MediatR;

namespace Social.Application.Features.Users.DeleteUserById;

public class DeleteUserByIdCommand : IRequest<DeleteUserByIdResponse>
{
    public int UserId { get; set; }
}