using MediatR;

namespace Application.Features.Messages.MarkMessageAsRead
{
    public class MarkMessageAsReadCommand : IRequest<MarkMessageAsReadResponse>
    {
        public int MessageId { get; set; }
    }
}
