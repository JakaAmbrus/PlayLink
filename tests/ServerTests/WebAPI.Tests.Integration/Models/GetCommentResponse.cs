using Application.Features.Comments.Common;

namespace WebAPI.Tests.Integration.Models
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
