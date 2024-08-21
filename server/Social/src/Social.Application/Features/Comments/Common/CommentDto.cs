namespace Social.Application.Features.Comments.Common
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public int AppUserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public int LikesCount { get; set; }
        public string ProfilePictureUrl { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime TimeCommented { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public bool IsAuthorized { get; set; }
    }
}
