namespace WebAPI.Entities
{
    public class FriendRequest
    {
        public int FriendRequestId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public bool? HasAccepted { get; set; }
        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }
        public DateTime DateSent { get; set; } = DateTime.UtcNow;
    }
}
