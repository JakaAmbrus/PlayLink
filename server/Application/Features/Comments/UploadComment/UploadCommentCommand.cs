using Application.Features.Comments.Common;
using MediatR;

namespace Application.Features.Comments.UploadComment
{
    public class UploadCommentCommand : IRequest<UploadCommentResponse>
    {
        public CommentContentDto CommentContent { get; set; }
    }
}
