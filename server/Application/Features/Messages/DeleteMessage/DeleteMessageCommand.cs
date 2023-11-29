using MediatR;

namespace Application.Features.Messages.DeleteMessage
{
    public class DeleteMessageCommand : IRequest<DeleteMessageResponse>
    {
        public int PrivateMessageId { get; set; }
    }
}
