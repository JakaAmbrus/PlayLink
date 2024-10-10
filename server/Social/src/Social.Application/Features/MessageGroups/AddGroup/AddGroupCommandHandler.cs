using Social.Domain.Entities;
using MediatR;
using Social.Application.Features.MessageGroups.Common;
using Social.Application.Interfaces;

namespace Social.Application.Features.MessageGroups.AddGroup
{
    public class AddGroupCommandHandler : IRequestHandler<AddGroupCommand, GroupDto>
    {
        private readonly IApplicationDbContext _context;

        public AddGroupCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GroupDto> Handle(AddGroupCommand request, CancellationToken cancellationToken)
        {
            var group = new Group
            {
                Name = request.GroupName
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync(cancellationToken);

            return new GroupDto
            {
                Name = group.Name,
                Connections = group.Connections.Select(c => new ConnectionDto
                {
                    ConnectionId = c.ConnectionId,
                    Username = c.Username
                }).ToList()
            };
        }
    }
}
