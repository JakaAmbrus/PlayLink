using Application.Exceptions;
using Application.Features.Messages.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Messages.GetMessageThread
{
    public class GetMessageThreadQueryHandler : IRequestHandler<GetMessageThreadQuery, GetMessageThreadResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetMessageThreadQueryHandler(IApplicationDbContext context, IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
        }

        public async Task<GetMessageThreadResponse> Handle(GetMessageThreadQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _context.Users.FindAsync(request.AuthUserId, cancellationToken)
                ?? throw new NotFoundException("User not found");

            var messages = await _context.PrivateMessages
                .Include(m => m.Sender)
                .Include(m => m.Recipient)
                .Where(m => m.RecipientUsername == currentUser.UserName 
                    && m.RecipientDeleted == false
                    && m.SenderUsername == request.RecipientUsername || m.RecipientUsername == request.RecipientUsername 
                    && m.SenderDeleted == false
                    && m.SenderUsername == currentUser.UserName)
                .OrderBy(m => m.PrivateMessageSent)
                .ToListAsync(cancellationToken);

            var unreadMessages = messages
                .Where(m => m.DateRead == null && m.RecipientUsername == currentUser.UserName)
                .ToList();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
            var messageDtos = messages.Select(m => new MessageDto
            {
                PrivateMessageId = m.PrivateMessageId,
                SenderUsername = m.SenderUsername,
                SenderProfilePictureUrl = m.Sender.ProfilePictureUrl,
                RecipientUsername = m.RecipientUsername,
                RecipientProfilePictureUrl = m.Recipient.ProfilePictureUrl,
                Content = m.Content,
                DateRead = m.DateRead.HasValue ? m.DateRead.Value.ToUniversalTime() : (DateTime?)null,
                PrivateMessageSent = m.PrivateMessageSent.ToUniversalTime(),
                SenderGender = currentUser.Gender,
                RecipientGender = m.Recipient.Gender,
                SenderFullName = m.Sender.FullName,
                RecipientFullName = m.Recipient.FullName
            }).ToList();


            return new GetMessageThreadResponse { Messages = messageDtos };
        }
    }
}
