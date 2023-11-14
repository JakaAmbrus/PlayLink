using Domain.Enums;

namespace Domain.Entities
{
    public class FriendRequest
    {
        public int FriendRequestId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        public DateTime TimeSent { get; set; } = DateTime.UtcNow;
        public FriendRequestStatus Status { get; set; } = FriendRequestStatus.Pending;

        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }
    }
}
