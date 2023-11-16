using MediatR;

namespace Application.Features.Comments.DeleteComment
{
    public class DeleteCommentCommand : IRequest<DeleteCommentResponse>
    {
        public int CommentId { get; set; }
        public DeleteCommentCommand(int commentId)
        {
            CommentId = commentId;
        }
    }
}
