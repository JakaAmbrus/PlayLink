using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.MessageGroups.GetConnection
{
    public class GetConnectionQueryHandler : IRequestHandler<GetConnectionQuery, Connection>
    {
        private readonly IApplicationDbContext _context;

        public GetConnectionQueryHandler(IApplicationDbContext context)
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
