using MediatR;

namespace Social.Application.Features.Likes.GetPostLikes
{
    public class GetPostLikesQuery : IRequest<GetPostLikesResponse>
    {
        public int PostId { get; set; }
        public int AuthUserId { get; set; }
    }
}
