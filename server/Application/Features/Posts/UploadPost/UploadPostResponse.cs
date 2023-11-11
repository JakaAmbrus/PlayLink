using Application.Features.Posts.Common;

namespace Application.Features.Posts.UploadPost
{
    public class UploadPostResponse 
    {
        public int? PostId { get; set; }
        public PostDto PostDto { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
