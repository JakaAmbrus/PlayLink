using MediatR;
using Social.Application.Features.MessageGroups.Common;

namespace Social.Application.Features.MessageGroups.GetMessageGroup
{
    public class GetMessageGroupQuery : IRequest<GroupDto>
    {
        public string GroupName { get; set; }
    }
}
