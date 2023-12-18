using MediatR;

namespace Application.Features.Likes.GetCommentLikes
{
    public class GetCommentLikesQuery : IRequest<GetCommentLikesResponse>
    {
        public int CommentId { get; set; }
    }
}
