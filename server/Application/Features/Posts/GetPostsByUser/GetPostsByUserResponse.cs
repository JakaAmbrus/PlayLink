using Application.Features.Posts.Common;
using Application.Utils;

namespace Application.Features.Posts.GetPostsByUser
{
    public class GetPostsByUserResponse 
    {
        public PagedList<PostDto> Posts { get; set; }
    }
}
