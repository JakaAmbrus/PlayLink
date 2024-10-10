using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Features.MessageGroups.Common;
using Social.Application.Interfaces;

namespace Social.Application.Features.MessageGroups.GetMessageGroup
{
    public class GetMessageGroupQueryHandler : IRequestHandler<GetMessageGroupQuery, GroupDto>
    {
        private readonly IApplicationDbContext _context;

        public GetMessageGroupQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GroupDto> Handle(GetMessageGroupQuery request, CancellationToken cancellationToken)
        {
            var group = await _context.Groups
                .AsNoTracking()
                .Include(g => g.Connections)
                .Where(g => g.Name == request.GroupName)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (group == null)
            {
                return null;
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
