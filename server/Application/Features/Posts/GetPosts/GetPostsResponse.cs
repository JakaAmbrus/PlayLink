using Application.Features.Posts.Common;
using Application.Utils;

namespace Application.Features.Posts.GetPosts
{
    public class GetPostsResponse
    {
        public PagedList<PostDto> Posts { get; set; }
    }
}
