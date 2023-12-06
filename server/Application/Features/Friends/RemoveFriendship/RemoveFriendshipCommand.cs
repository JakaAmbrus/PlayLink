using MediatR;

namespace Application.Features.Friends.RemoveFriendship
{
    public class RemoveFriendshipCommand : IRequest<RemoveFriendshipResponse>
    {
        public string ProfileUsername { get; set; } 
        public int AuthUserId { get; set; }
    }
}
