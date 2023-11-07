
namespace server.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
       /* [Required]
        [MaxLength(100)]*/
        public string Content { get; set; }
        public DateTime DateCommented { get; set; } = DateTime.UtcNow;

        //linked to post
        public int PostId { get; set; }
        public Post Post { get; set; }

        //linked to user
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        //linked to like
        public ICollection<Like> Likes { get; set; }    
    }
}
