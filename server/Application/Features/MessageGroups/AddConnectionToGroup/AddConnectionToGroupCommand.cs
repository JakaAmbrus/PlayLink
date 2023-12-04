using Application.Features.MessageGroups.Common;
using MediatR;

namespace Application.Features.MessageGroups.AddConnectionToGroup
{
    public class AddConnectionToGroupCommand : IRequest<GroupDto>
    {
        public string GroupName { get; set; }
        public ConnectionDto ConnectionDto { get; set; }
    }
}
