namespace Application.Features.Messages.Common
{
    public class MessageDto
    {
        public int PrivateMessageId { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string SenderProfilePictureUrl { get; set; }
        public string RecipientProfilePictureUrl { get; set; }
        public string SenderUsername { get; set; }
        public string RecipientUsername { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime PrivateMessageSent { get; set; }
    }
}
