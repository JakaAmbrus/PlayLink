using Social.Application.Features.Posts.Common;
using Social.Application.Utils;

namespace Social.Application.Features.Posts.GetPostsByUser
{
    public class GetPostsByUserResponse 
    {
        public PagedList<PostDto> Posts { get; set; }
    }
}
