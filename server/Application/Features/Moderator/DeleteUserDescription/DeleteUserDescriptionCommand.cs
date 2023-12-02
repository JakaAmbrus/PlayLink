using MediatR;

namespace Application.Features.Moderator.DeleteUserDescription
{
    public class DeleteUserDescriptionCommand : IRequest<DeleteUserDescriptionResponse>
    {
        public string Username { get; set; }
    }
}
