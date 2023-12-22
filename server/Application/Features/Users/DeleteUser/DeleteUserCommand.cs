using MediatR;

namespace Application.Features.Users.DeleteUser
{
    public class DeleteUserCommand : IRequest<DeleteUserResponse>
    {
        public int AuthUserId { get; set; }
        public IEnumerable<string> AuthUserRoles { get; set; }
    }
}
