using MediatR;

namespace Social.Application.Features.Friends.SendFriendRequest
{
    public class SendFriendRequestCommand : IRequest<SendFriendRequestResponse>
    {
        public string ReceiverUsername { get; set; }
        public int AuthUserId { get; set; }
    }
}
