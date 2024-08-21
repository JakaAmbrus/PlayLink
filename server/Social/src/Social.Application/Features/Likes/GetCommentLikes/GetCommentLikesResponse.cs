using Social.Application.Features.Likes.Common;

namespace Social.Application.Features.Likes.GetCommentLikes
{
    public class GetCommentLikesResponse
    {
        public List<LikedUserDto> LikedUsers { get; set; }
    }
}
