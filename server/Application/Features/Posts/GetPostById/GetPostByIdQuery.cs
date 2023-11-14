using Application.Features.Posts.Common;
using MediatR;

namespace Application.Features.Posts.GetPostById
{
    public class GetPostByIdQuery : IRequest<PostDto>
    {
        public int PostId { get; set; }
        public GetPostByIdQuery(int postId) {
            PostId = postId;
        }
    }
}
