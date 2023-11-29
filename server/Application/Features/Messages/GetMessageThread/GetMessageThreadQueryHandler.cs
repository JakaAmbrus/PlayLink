using Application.Exceptions;
using Application.Features.Messages.Common;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Messages.GetMessageThread
{
    public class GetMessageThreadQueryHandler : IRequestHandler<GetMessageThreadQuery, GetMessageThreadResponse>
    {
        private readonly DataContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public GetMessageThreadQueryHandler(DataContext context, IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<GetMessageThreadResponse> Handle(GetMessageThreadQuery request, CancellationToken cancellationToken)
        {
            int authUserId = _authenticatedUserService.UserId;

            var currentUser = await _context.Users.FindAsync(authUserId, cancellationToken)
                ?? throw new NotFoundException("User not found");

            var messages = await _context.PrivateMessages
                .AsQueryable()
                .Where(m => m.RecipientUsername == currentUser.UserName && m.RecipientDeleted == false
                    && m.SenderUsername == request.RecipientUsername
                    || m.RecipientUsername == request.RecipientUsername && m.SenderDeleted == false
                    && m.SenderUsername == currentUser.UserName)
                .OrderBy(m => m.PrivateMessageSent)
                .Select(m => new MessageDto
                {
                    PrivateMessageId = m.PrivateMessageId,
                    SenderUsername = m.SenderUsername,
                    SenderProfilePictureUrl = m.Sender.ProfilePictureUrl,
                    RecipientUsername = m.RecipientUsername,
                    RecipientProfilePictureUrl = m.Recipient.ProfilePictureUrl,
                    Content = m.Content,
                    DateRead = m.DateRead,
                    PrivateMessageSent = m.PrivateMessageSent,
                    SenderGender = currentUser.Gender,
                    RecipientGender = m.Recipient.Gender,
                    SenderFullName = m.Sender.FullName,
                    RecipientFullName = m.Recipient.FullName
                })
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

            return new GetMessageThreadResponse { Messages = messages };
        }
    }
}
