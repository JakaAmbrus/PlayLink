namespace Domain.Entities
{
    public class Like
    {
        public int LikeId { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int? PostId { get; set; }
        public Post Post { get; set; }
        public int? CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
