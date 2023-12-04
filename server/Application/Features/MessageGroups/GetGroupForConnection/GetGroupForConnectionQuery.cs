using Application.Features.MessageGroups.Common;
using MediatR;

namespace Application.Features.MessageGroups.GetGroupForConnection
{
    public class GetGroupForConnectionQuery: IRequest<GroupDto>
    {
        public string ConnectionId { get; set; }
    }
}
