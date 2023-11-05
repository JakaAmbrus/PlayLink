namespace server.Entities
{
    public class GroupChat
    {
        public int GroupChatId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public ICollection<GroupMessage> GroupMessages { get; set; }
        public ICollection<GroupChatUser> GroupChatUsers { get; set; }


    }
}
