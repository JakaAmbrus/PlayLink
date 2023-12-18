using Application.Features.Likes.Common;

namespace Application.Features.Likes.GetCommentLikes
{
    public class GetCommentLikesResponse
    {
        public List<LikedUserDto> LikedUsers { get; set; }
    }
}
