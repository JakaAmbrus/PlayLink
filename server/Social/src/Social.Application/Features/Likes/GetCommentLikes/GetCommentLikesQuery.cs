using MediatR;

namespace Social.Application.Features.Likes.GetCommentLikes
{
    public class GetCommentLikesQuery : IRequest<GetCommentLikesResponse>
    {
        public int CommentId { get; set; }
        public int AuthUserId { get; set; }
    }
}
