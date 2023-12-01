using Application.Features.Comments.Common;
using MediatR;

namespace Application.Features.Comments.UploadComment
{
    public class UploadCommentCommand : IRequest<UploadCommentResponse>
    {
        public int PostId { get; set; }
        public CommentContentDto CommentContentDto { get; set; }
        public int AuthUserId { get; set; }

        public UploadCommentCommand(int postId, CommentContentDto commentContentDto, int authUserId)
        {
            PostId = postId;
            CommentContentDto = commentContentDto;
            AuthUserId = authUserId;
        }
    }
}
