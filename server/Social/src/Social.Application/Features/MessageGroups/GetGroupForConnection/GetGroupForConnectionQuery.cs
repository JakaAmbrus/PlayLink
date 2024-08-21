using MediatR;
using Social.Application.Features.MessageGroups.Common;

namespace Social.Application.Features.MessageGroups.GetGroupForConnection
{
    public class GetGroupForConnectionQuery: IRequest<GroupDto>
    {
        public string ConnectionId { get; set; }
    }
}
