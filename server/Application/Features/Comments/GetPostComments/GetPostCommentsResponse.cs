using Application.Features.Comments.Common;

namespace Application.Features.Comments.GetComments
{
    public class GetPostCommentsResponse
    {
        public List<CommentDto> Comments { get; set; }
    }
}
