namespace Domain.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        public string Description { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
        public string PhotoUrl { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Notification> Notifications { get; set; }

    }
}
