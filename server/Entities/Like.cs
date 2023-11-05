namespace server.Entities
{
    public class Like
    {
        public int LikeId { get; set; }

        // Linked to user
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        // Linked to post or comment
        public int? PostId { get; set; }
        public Post Post { get; set; }
        public int? CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
