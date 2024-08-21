using MediatR;
using Social.Application.Features.MessageGroups.Common;

namespace Social.Application.Features.MessageGroups.AddGroup
{
    public class AddGroupCommand : IRequest<GroupDto>
    { 
        public string GroupName { get; set;}
    }   
}
