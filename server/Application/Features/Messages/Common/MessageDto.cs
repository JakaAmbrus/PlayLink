namespace Application.Features.Messages.Common
{
    public class MessageDto
    {
        public int PrivateMessageId { get; set; }
        public string SenderUsername { get; set; }
        public string SenderFullName { get; set; }
        public string SenderGender { get; set; }
        public string SenderProfilePictureUrl { get; set; }
        public string RecipientUsername { get; set; }
        public string RecipientFullName { get; set; }
        public string RecipientGender { get; set; }
        public string RecipientProfilePictureUrl { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime PrivateMessageSent { get; set; }
    }
}
