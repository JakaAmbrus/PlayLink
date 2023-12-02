using MediatR;

namespace Application.Features.Likes.UnlikeComment
{
    public class UnlikeCommentCommand : IRequest<UnlikeCommentResponse>
    { 
        public int CommentId { get; set; }
        public int AuthUserId { get; set; }
    }
}
