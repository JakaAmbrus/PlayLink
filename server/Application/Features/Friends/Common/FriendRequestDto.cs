using Domain.Enums;

namespace Application.Features.Friends.Common
{
    public class FriendRequestDto
    {
        public int FriendRequestId { get; set; }
        public string SenderUsername { get; set; }
        public string SenderFullName { get; set; }
        public string SenderProfilePictureUrl { get; set; }
        public string SenderGender { get; set; }
        public string RecipientUsername { get; set; }
        public string RecipientFullName { get; set; }
        public string RecipientProfilePictureUrl { get; set; }
        public string RecipientGender { get; set; }
        public FriendRequestStatus Status { get; set; }
        public DateTime TimeSent { get; set; }
    }
}
