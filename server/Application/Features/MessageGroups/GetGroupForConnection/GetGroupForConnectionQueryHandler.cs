using Application.Exceptions;
using Application.Features.MessageGroups.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MessageGroups.GetGroupForConnection
{
    public class GetGroupForConnectionQueryHandler : IRequestHandler<GetGroupForConnectionQuery, GroupDto>
    {
        private readonly IApplicationDbContext _context;

        public GetGroupForConnectionQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GroupDto> Handle(GetGroupForConnectionQuery request, CancellationToken cancellationToken)
        {
            var group = await _context.Groups
                .Include(g => g.Connections)
                .Where(g => g.Connections.Any(c => c.ConnectionId == request.ConnectionId))
                .FirstOrDefaultAsync();

            if (group == null)
            {
                throw new NotFoundException("Group not found");
            }

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
