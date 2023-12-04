using Application.Features.MessageGroups.Common;
using MediatR;

namespace Application.Features.MessageGroups.GetMessageGroup
{
    public class GetMessageGroupQuery : IRequest<GroupDto>
    {
        public string GroupName { get; set; }
    }
}
