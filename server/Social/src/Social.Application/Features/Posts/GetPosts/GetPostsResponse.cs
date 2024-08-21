using Social.Application.Features.Posts.Common;
using Social.Application.Utils;

namespace Social.Application.Features.Posts.GetPosts
{
    public class GetPostsResponse
    {
        public PagedList<PostDto> Posts { get; set; }
    }
}
