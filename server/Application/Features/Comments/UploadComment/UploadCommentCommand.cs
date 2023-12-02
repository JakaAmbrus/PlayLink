using Application.Features.Comments.Common;
using MediatR;

namespace Application.Features.Comments.UploadComment
{
    public class UploadCommentCommand : IRequest<UploadCommentResponse>
    {
        public CommentUploadDto Comment { get; set; }
        public int AuthUserId { get; set; }
    }
}
