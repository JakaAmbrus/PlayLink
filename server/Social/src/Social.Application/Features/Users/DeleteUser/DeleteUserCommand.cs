using MediatR;

namespace Social.Application.Features.Users.DeleteUser
{
    public class DeleteUserCommand : IRequest<DeleteUserResponse>
    {
        public int UserId { get; set; }
        
        public bool? SignUpFailure { get; set; }
    }
}
