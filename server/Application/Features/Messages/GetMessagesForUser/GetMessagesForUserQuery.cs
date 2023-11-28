using Application.Utils;
using MediatR;

namespace Application.Features.Messages.GetMessagesForUser
{
    public class GetMessagesForUserQuery : IRequest<GetMessagesForUserResponse>
    {
        public MessageParams Params { get; set; } = new MessageParams();

    }
}
