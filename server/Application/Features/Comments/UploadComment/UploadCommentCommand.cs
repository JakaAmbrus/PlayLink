using MediatR;

namespace Application.Features.Comments.UploadComment
{
    public class UploadCommentCommand : IRequest<UploadCommentResponse>
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public int AuthUserId { get; set; }
    }
}
