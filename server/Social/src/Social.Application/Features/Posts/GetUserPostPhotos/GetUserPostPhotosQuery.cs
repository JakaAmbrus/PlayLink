using MediatR;

namespace Social.Application.Features.Posts.GetUserPostPhotos
{
    public class GetUserPostPhotosQuery : IRequest<GetUserPostPhotosResponse>
    {
        public string Username { get; set; }
    }
}
