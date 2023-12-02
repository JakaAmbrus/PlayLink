using Application.Features.Messages.Common;
using MediatR;

namespace Application.Features.Messages.SendMessage
{
    public class SendMessageCommand : IRequest<SendMessageResponse>
    {
        public CreateMessageDto CreateMessageDto { get; set; }
        public int AuthUserId { get; set; }
    }
}
