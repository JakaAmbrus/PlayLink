using Application.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Messages.DeleteMessage
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, DeleteMessageResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public DeleteMessageCommandHandler(IApplicationDbContext context, IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<DeleteMessageResponse> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            int authUserId = _authenticatedUserService.UserId;

            var user = await _context.Users
                .FindAsync(new object[] { authUserId }, cancellationToken)
                ?? throw new NotFoundException("User not found");

            var message = await _context.PrivateMessages
                .FindAsync(new object[] { request.PrivateMessageId }, cancellationToken)
                ?? throw new NotFoundException("Message not found");

            if (message.SenderId != user.Id && message.RecipientId != user.Id)
            {
                throw new UnauthorizedException("You are not authorized to delete this message");
            }

            if (message.SenderId == user.Id)
            {
                message.SenderDeleted = true;
            }

            if (message.RecipientId == user.Id)
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
