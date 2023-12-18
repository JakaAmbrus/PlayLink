using MediatR;

namespace Application.Features.Likes.GetPostLikes
{
    public class GetPostLikesQuery : IRequest<GetPostLikesResponse>
    {
        public int PostId { get; set; }
    }
}
