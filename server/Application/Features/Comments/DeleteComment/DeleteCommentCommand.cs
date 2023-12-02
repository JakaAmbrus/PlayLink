using MediatR;

namespace Application.Features.Comments.DeleteComment
{
    public class DeleteCommentCommand : IRequest<DeleteCommentResponse>
    {
        public int CommentId { get; set; }
        public int AuthUserId { get; set; }
        public IEnumerable<string> AuthUserRoles { get; set; }

    }
}
