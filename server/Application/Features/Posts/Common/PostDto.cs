namespace Application.Features.Posts.Common
{
    public class PostDto
    {
        public int PostId { get; set; }
        public int AppUserId { get; set; }
        public string Description { get; set; }
        public DateTime DatePosted { get; set; }
        public string PhotoUrl { get; set; }
    }
}
