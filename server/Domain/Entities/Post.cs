namespace Domain.Entities
{
    public class Post
    {
        public int PostId { get; set; }

        public string Description { get; set; }

        public DateTime DatePosted { get; set; } = DateTime.UtcNow;

        // If a photo exists, then the post contains a photo that gets added to the user gallery
        public int? PhotoId { get; set; }
        public Photo Photo { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Notification> Notifications { get; set; }

    }
}
