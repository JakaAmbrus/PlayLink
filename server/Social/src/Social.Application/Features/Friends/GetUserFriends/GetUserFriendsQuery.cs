using MediatR;

namespace Social.Application.Features.Friends.GetUserFriends
{
    public class GetUserFriendsQuery : IRequest<GetUserFriendsResponse>
    {
        public int AuthUserId { get; set; }
    }
}
