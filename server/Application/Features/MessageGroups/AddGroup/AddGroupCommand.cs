using Application.Features.MessageGroups.Common;
using MediatR;

namespace Application.Features.MessageGroups.AddGroup
{
    public class AddGroupCommand : IRequest<GroupDto>
    { 
        public string GroupName { get; set;}
    }   
}
