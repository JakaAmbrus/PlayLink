using Application.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Features.MessageGroups.RemoveConnection
{
    public class RemoveConnectionCommandHandler : IRequestHandler<RemoveConnectionCommand, RemoveConnectionResponse>
    {
        private readonly IApplicationDbContext _context;

        public RemoveConnectionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RemoveConnectionResponse> Handle(RemoveConnectionCommand request, CancellationToken cancellationToken)
        {
            var connection = await _context.Connections.FindAsync(new object[] { request.ConnectionId }, cancellationToken) 
                ?? throw new NotFoundException("Connection not found");

            _context.Connections.Remove(connection);

            await _context.SaveChangesAsync(cancellationToken);

            return new RemoveConnectionResponse { Succeeded = true };
        }
    }
}
