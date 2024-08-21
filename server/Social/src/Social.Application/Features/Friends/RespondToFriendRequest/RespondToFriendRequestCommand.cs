using MediatR;
using Social.Application.Features.Friends.Common;

namespace Social.Application.Features.Friends.RespondToFriendRequest
{
    public class RespondToFriendRequestCommand : IRequest<RespondToFriendRequestResponse>
    {
        public FriendRequestResponseDto FriendRequestResponse { get; set; }
        public int AuthUserId { get; set; }
    }
}
