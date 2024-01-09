using Application.Features.Posts.Common;

namespace WebAPI.Tests.Integration.Models
{
    public class GetPostsResponse
    {
        public List<PostDto> Posts { get; set; }

        public GetPostsResponse()
        {
            Posts = new List<PostDto>();
        }
    }
}
