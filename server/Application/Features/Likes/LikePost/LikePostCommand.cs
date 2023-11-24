using MediatR;

namespace Application.Features.Likes.LikePost
{
    public class LikePostCommand : IRequest<LikePostResponse>
    {
        public int PostId { get; set; }
    }
}
