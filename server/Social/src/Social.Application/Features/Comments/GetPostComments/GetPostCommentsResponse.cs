using Social.Application.Features.Comments.Common;
using Social.Application.Utils;

namespace Social.Application.Features.Comments.GetComments
{
    public class GetPostCommentsResponse
    {
        public PagedList<CommentDto> Comments { get; set; }
    }
}
