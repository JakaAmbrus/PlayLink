using MediatR;

namespace Social.Application.Features.Likes.UnlikePost
{
    public class UnlikePostCommand : IRequest<UnlikePostResponse>
    {
        public int PostId { get; set; }
        public int AuthUserId { get; set; }
    }
}
