using Application.Features.Friends.Common;
using MediatR;

namespace Application.Features.Friends.RespondToFriendRequest
{
    public class RespondToFriendRequestCommand : IRequest<RespondToFriendRequestResponse>
    {
        public FriendRequestResponseDto FriendRequestResponse { get; set; }
        public int AuthUserId { get; set; }
    }
}
