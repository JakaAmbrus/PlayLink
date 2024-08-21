using MediatR;

namespace Social.Application.Features.Messages.MarkMessageAsRead
{
    public class MarkMessageAsReadCommand : IRequest<MarkMessageAsReadResponse>
    {
        public int MessageId { get; set; }
    }
}
