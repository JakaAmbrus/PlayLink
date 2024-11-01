using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;
using Social.Application.Interfaces;

namespace Social.Application.Features.MessageGroups.GetConnection
{
    public class GetConnectionQueryHandler : IRequestHandler<GetConnectionQuery, Connection>
    {
        private readonly ISocialDbContext _context;

        public GetConnectionQueryHandler(ISocialDbContext context)
        {
            _context = context;
        }

        public async Task<Connection> Handle(GetConnectionQuery request, CancellationToken cancellationToken)
        {
            var connection = await _context.Connections.FindAsync(new object[] { request.ConnectionId }, cancellationToken) 
                ?? throw new NotFoundException("Connection not found");

            return connection;
        }
    }
}
