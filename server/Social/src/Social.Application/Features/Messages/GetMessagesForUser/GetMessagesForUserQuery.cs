using MediatR;
using Social.Application.Utils;

namespace Social.Application.Features.Messages.GetMessagesForUser
{
    public class GetMessagesForUserQuery : IRequest<GetMessagesForUserResponse>
    {
        public MessageParams Params { get; set; } = new MessageParams();
        public int AuthUserId { get; set; } 

    }
}
