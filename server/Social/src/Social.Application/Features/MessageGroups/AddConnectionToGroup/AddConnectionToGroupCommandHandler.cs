using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Features.MessageGroups.Common;
using Social.Application.Interfaces;

namespace Social.Application.Features.MessageGroups.AddConnectionToGroup
{
    public class AddConnectionToGroupCommandHandler : IRequestHandler<AddConnectionToGroupCommand, GroupDto>
    {
        private readonly IApplicationDbContext _context;

        public AddConnectionToGroupCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GroupDto> Handle(AddConnectionToGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Name == request.GroupName, cancellationToken) 
                ?? throw new NotFoundException("Group not found");

            var connection = new Connection
            {
                ConnectionId = request.ConnectionDto.ConnectionId,
                Username = request.ConnectionDto.Username
            };

            group.Connections.Add(connection);

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
