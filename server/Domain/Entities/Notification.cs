namespace Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int SenderId { get; set; } 
        public string Type { get; set; } // if it is a like or comment
        public bool IsRead { get; set; } = false;
        public DateTime NotificationDate { get; set; } = DateTime.UtcNow; 
        public int PostId { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public Post Post { get; set; }
    }
}
