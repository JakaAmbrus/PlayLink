using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Features.Messages.Common;
using Social.Application.Interfaces;
using Social.Application.Utils;
using Social.Domain.Enums;

namespace Social.Application.Features.Messages.GetMessagesForUser
{
    public class GetMessagesForUserQueryHandler : IRequestHandler<GetMessagesForUserQuery, GetMessagesForUserResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetMessagesForUserQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetMessagesForUserResponse> Handle(GetMessagesForUserQuery request, CancellationToken cancellationToken)
        {
            Enum.TryParse<MessageStatus>(request.Params.Container, true, out var status);

            var user = await _context.Users
                .FindAsync(new object[] { request.AuthUserId }, cancellationToken)
                ?? throw new NotFoundException("User not found");

            IQueryable<PrivateMessage> messageQuery;

            switch (status)
            {
                case MessageStatus.Inbox:
                    messageQuery = _context.PrivateMessages
                        .Where(u => u.RecipientUsername == user.UserName && u.RecipientDeleted == false);
                    break;

                case MessageStatus.Outbox:
                    messageQuery = _context.PrivateMessages
                        .Where(u => u.SenderUsername == user.UserName && u.SenderDeleted == false);
                    break;
                
                case MessageStatus.Unread:
                    messageQuery = _context.PrivateMessages
                        .Where(u => u.SenderUsername == user.UserName && u.SenderDeleted == false);
                    break;

                default:
                    throw new BadRequestException("Message status not recognized");
            }

            var messages = messageQuery
                .AsNoTracking()
                .OrderByDescending(m => m.PrivateMessageSent)
                .Select(m => new MessageDto
                {
                    PrivateMessageId = m.PrivateMessageId,
                    SenderUsername = m.SenderUsername,
                    SenderProfilePictureUrl = m.Sender.ProfilePictureUrl,
                    RecipientUsername = m.RecipientUsername,
                    RecipientProfilePictureUrl = m.Recipient.ProfilePictureUrl,
                    Content = m.Content,
                    DateRead = m.DateRead.HasValue ? m.DateRead.Value.ToUniversalTime() : null,
                    PrivateMessageSent = m.PrivateMessageSent.ToUniversalTime(),
                    SenderGender = user.Gender,
                    RecipientGender = m.Sender.Gender,
                    SenderFullName = m.Sender.FullName,
                    RecipientFullName = m.Recipient.FullName
                });

            var pagedMessages = await PagedList<MessageDto>
                .CreateAsync(messages, request.Params.PageNumber, request.Params.PageSize);

            return new GetMessagesForUserResponse { Messages = pagedMessages };
        }
    }
}
