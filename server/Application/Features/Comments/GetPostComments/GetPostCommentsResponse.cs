using Application.Features.Comments.Common;
using Application.Utils;

namespace Application.Features.Comments.GetComments
{
    public class GetPostCommentsResponse
    {
        public PagedList<CommentDto> Comments { get; set; }
    }
}
