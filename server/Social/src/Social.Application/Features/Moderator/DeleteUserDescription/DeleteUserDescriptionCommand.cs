using MediatR;

namespace Social.Application.Features.Moderator.DeleteUserDescription
{
    public class DeleteUserDescriptionCommand : IRequest<DeleteUserDescriptionResponse>
    {
        public string Username { get; set; }
    }
}
