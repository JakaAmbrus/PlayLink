using MediatR;

namespace Application.Features.Likes.LikeComment
{
    public class LikeCommentCommand : IRequest<LikeCommentResponse>
    {
        public int CommentId { get; set; }
        public int AuthUserId { get; set; }
    }
}
