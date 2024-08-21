namespace Social.Domain.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public string Description { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
        public string PhotoPublicId { get; set; }
        public string PhotoUrl { get; set; }
        
        public ICollection<Comment> Comments { get; set; }
        public int CommentsCount { get; set; } = 0;
        public ICollection<Like> Likes { get; set; }
        public int LikesCount { get; set; } = 0;
    }
}
