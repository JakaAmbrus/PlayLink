using MediatR;
using Social.Application.Utils;

namespace Social.Application.Features.Posts.GetPosts
{
    public class GetPostsQuery : IRequest<GetPostsResponse>
    { 
        public PaginationParams Params { get; set; } = new PaginationParams();
        public int AuthUserId { get; set; }
        public IEnumerable<string> AuthUserRoles { get; set; }
    }
}
