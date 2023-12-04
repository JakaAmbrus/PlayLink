using MediatR;

namespace Application.Features.MessageGroups.MarkMessageAsRead
{
    public class MarkMessageAsReadCommand : IRequest<MarkMessageAsReadResponse>
    {
        public int MessageId { get; set; }
    }
}
