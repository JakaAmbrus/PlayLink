using MediatR;

namespace Application.Features.Posts.GetUserPostPhotos
{
    public class GetUserPostPhotosQuery : IRequest<GetUserPostPhotosResponse>
    {
        public string Username { get; set; }
    }
}
