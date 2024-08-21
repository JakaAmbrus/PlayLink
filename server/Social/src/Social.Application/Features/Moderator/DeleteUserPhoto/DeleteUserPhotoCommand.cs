using MediatR;

namespace Social.Application.Features.Moderator.DeleteUserPhoto
{
    public class DeleteUserPhotoCommand : IRequest<DeleteUserPhotoResponse>
    {
        public string Username { get; set; }
    }
}
