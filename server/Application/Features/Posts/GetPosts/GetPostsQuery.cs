using Application.Features.Posts.Common;
using MediatR;

namespace Application.Features.Posts.GetPosts
{
    public class GetPostsQuery : IRequest<List<PostDto>>
    { 
    }
}
