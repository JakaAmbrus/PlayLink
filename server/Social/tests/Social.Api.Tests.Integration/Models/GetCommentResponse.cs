using Social.Application.Features.Comments.Common;

namespace Social.Api.Tests.Integration.Models
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
