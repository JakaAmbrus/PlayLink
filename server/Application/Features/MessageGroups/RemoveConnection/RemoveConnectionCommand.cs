using MediatR;

namespace Application.Features.MessageGroups.RemoveConnection
{
    public class RemoveConnectionCommand : IRequest<RemoveConnectionResponse>
    {
        public string ConnectionId { get; set; }
    }
}
