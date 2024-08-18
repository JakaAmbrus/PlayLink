using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Messages.MarkMessageAsRead
{
    public class MarkMessageAsReadCommandHandler : IRequestHandler<MarkMessageAsReadCommand, MarkMessageAsReadResponse>
    {
        private readonly IApplicationDbContext _context;

        public MarkMessageAsReadCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MarkMessageAsReadResponse> Handle(MarkMessageAsReadCommand request, CancellationToken cancellationToken)
        {

            var message = await _context.PrivateMessages.FindAsync(new object[] { request.MessageId }, cancellationToken)
                ?? throw new NotFoundException("Message not found");


            message.DateRead = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new MarkMessageAsReadResponse { MessageMarked = true };
        }
    }
}
