using MediatR;
using Social.Application.Features.MessageGroups.Common;

namespace Social.Application.Features.MessageGroups.AddConnectionToGroup
{
    public class AddConnectionToGroupCommand : IRequest<GroupDto>
    {
        public string GroupName { get; set; }
        public ConnectionDto ConnectionDto { get; set; }
    }
}
