using MediatR;

namespace Application.Features.Friends.GetFriendRequests
{
    public class GetFriendRequestsQuery : IRequest<GetFriendRequestsResponse>
    {
        public int AuthUserId { get; set; }
    }
}
