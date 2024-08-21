using MediatR;
using Social.Application.Features.Messages.Common;

namespace Social.Application.Features.Messages.SendMessage
{
    public class SendMessageCommand : IRequest<SendMessageResponse>
    {
        public CreateMessageDto CreateMessageDto { get; set; }
        public int AuthUserId { get; set; }
    }
}
