namespace Domain.Entities
{
    public class GroupMessage
    {
        public int GroupMessageId { get; set; }
        public string Message { get; set; }
        public DateTime MessageTime { get; set; } = DateTime.UtcNow;

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int GroupChatId { get; set; }
        public GroupChat GroupChat { get; set; }     
    }
}
