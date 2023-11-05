namespace server.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public virtual AppUser Sender { get; set; }
        public int RecipientId { get; set; }
        public virtual AppUser Recipient { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}
