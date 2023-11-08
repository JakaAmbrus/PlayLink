namespace WebAPI.Entities
{
    public class GroupChatUser
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int GroupChatId { get; set; }
        public GroupChat GroupChat { get; set; }
    }
}
