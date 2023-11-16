namespace Application.Features.Comments.Common
{
    public class CommentDto
    {
        public int AppUserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
    }
}
