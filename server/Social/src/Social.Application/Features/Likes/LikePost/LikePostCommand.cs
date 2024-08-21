using MediatR;

namespace Social.Application.Features.Likes.LikePost
{
    public class LikePostCommand : IRequest<LikePostResponse>
    {
        public int PostId { get; set; }
        public int AuthUserId { get; set; }
    }
}
