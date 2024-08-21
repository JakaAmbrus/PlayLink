using Social.Application.Features.Likes.Common;

namespace Social.Application.Features.Likes.GetPostLikes
{
    public class GetPostLikesResponse
    {
        public List<LikedUserDto> LikedUsers { get; set; }
    }
}
