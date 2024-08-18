using Application.Features.Messages.Common;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Messages.GetMessageThread
{
    public class GetMessageThreadQueryHandler : IRequestHandler<GetMessageThreadQuery, GetMessageThreadResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetMessageThreadQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetMessageThreadResponse> Handle(GetMessageThreadQuery request, CancellationToken cancellationToken)
        {
            var authUser = await _context.Users.FindAsync(new object[] { request.AuthUserId, cancellationToken }, cancellationToken)
                ?? throw new NotFoundException("Authorized user not found");

            var profileUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.ProfileUsername, cancellationToken)
                ?? throw new NotFoundException("Profile user not found");

            var messages = await _context.PrivateMessages
                .Include(m => m.Sender)
                .Include(m => m.Recipient)
                .Where(m => m.RecipientUsername == authUser.UserName 
                    && m.RecipientDeleted == false
                    && m.SenderUsername == request.ProfileUsername || m.RecipientUsername == request.ProfileUsername
                    && m.SenderDeleted == false
                    && m.SenderUsername == authUser.UserName)
                .OrderBy(m => m.PrivateMessageSent)
                .ToListAsync(cancellationToken);

            var unreadMessages = messages
                .Where(m => m.DateRead == null && m.RecipientUsername == authUser.UserName)
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
                SenderGender = authUser.Gender,
                RecipientGender = m.Recipient.Gender,
                SenderFullName = m.Sender.FullName,
                RecipientFullName = m.Recipient.FullName
            }).ToList();


            return new GetMessageThreadResponse { Messages = messageDtos };
        }
    }
}
