
namespace Domain.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime DateCommented { get; set; } = DateTime.UtcNow;
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<Like> Likes { get; set; }    
    }
}
