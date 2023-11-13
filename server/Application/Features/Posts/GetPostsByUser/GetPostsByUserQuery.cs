using Application.Features.Posts.Common;
using MediatR;

namespace Application.Features.Posts.GetPostsByUser
{
    public class GetPostsByUserQuery : IRequest<IEnumerable<PostDto>>
    {
        public int AppUserId { get; set; }

        public GetPostsByUserQuery(int userId)
        {
            AppUserId = userId;
        }
    }
}
