using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Messages.DeleteMessage
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, DeleteMessageResponse>
    {
        private readonly IApplicationDbContext _context;

        public DeleteMessageCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteMessageResponse> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var message = await _context.PrivateMessages
                .FindAsync(new object[] { request.PrivateMessageId }, cancellationToken)
                ?? throw new NotFoundException("Message not found");

            if (message.SenderId != request.AuthUserId && message.RecipientId != request.AuthUserId)
            {
                throw new UnauthorizedException("You are not authorized to delete this message");
            }

            if (message.SenderId == request.AuthUserId)
            {
                message.SenderDeleted = true;
            }

            if (message.RecipientId == request.AuthUserId)
            { 
                message.RecipientDeleted = true; 
            }

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                _context.PrivateMessages.Remove(message);
            }

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            return new DeleteMessageResponse { MessageDeleted = success };
        }
    }
}
