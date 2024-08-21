using Social.Application.Features.Friends.Common;

namespace Social.Application.Features.Friends.RespondToFriendRequest
{
    public class RespondToFriendRequestResponse
    {
        public bool RequestAccepted { get; set; }
        public FriendDto FriendDto { get; set; }
    }
}
