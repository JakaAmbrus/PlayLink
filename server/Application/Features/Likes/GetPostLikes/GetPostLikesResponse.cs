using Application.Features.Likes.Common;

namespace Application.Features.Likes.GetPostLikes
{
    public class GetPostLikesResponse
    {
        public List<LikedUserDto> LikedUsers { get; set; }
    }
}
