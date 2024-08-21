using MediatR;

namespace Social.Application.Features.MessageGroups.RemoveConnection
{
    public class RemoveConnectionCommand : IRequest<RemoveConnectionResponse>
    {
        public string ConnectionId { get; set; }
    }
}
