using MediatR;

namespace Application.Features.Moderator.DeleteUserPhoto
{
    public class DeleteUserPhotoCommand : IRequest<DeleteUserPhotoResponse>
    {
        public string Username { get; set; }
    }
}
