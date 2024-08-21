using MediatR;
using Social.Application.Features.Comments.Common;

namespace Social.Application.Features.Comments.UploadComment
{
    public class UploadCommentCommand : IRequest<UploadCommentResponse>
    {
        public CommentUploadDto Comment { get; set; }
        public int AuthUserId { get; set; }
    }
}
