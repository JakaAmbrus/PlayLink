using Social.Application.Features.Comments.Common;

namespace Social.FunctionalTests.Models
{
    public class GetCommentResponse
    {
        public List<CommentDto> Comments { get; set; }

        public GetCommentResponse()
        {
            Comments = new List<CommentDto>();
        }
    }
}
