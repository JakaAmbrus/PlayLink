using MediatR;

namespace Application.Features.Friends.RemoveFriendRequest
{
    public class RemoveFriendRequestCommand : IRequest<RemoveFriendRequestResponse>
    {
        public int FriendRequestId { get; set; }
        public int AuthUserId { get; set; }
    }
}
