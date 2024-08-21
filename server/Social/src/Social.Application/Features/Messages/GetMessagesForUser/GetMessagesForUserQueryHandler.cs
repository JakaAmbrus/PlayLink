using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;
using Social.Application.Features.Messages.Common;
using Social.Application.Interfaces;
using Social.Application.Utils;

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
            var user = await _context.Users
                .FindAsync(new object[] { request.AuthUserId }, cancellationToken)
                ?? throw new NotFoundException("User not found");

            IQueryable<PrivateMessage> messageQuery;

            switch (request.Params.Container)
            {
                case "Inbox":
                    messageQuery = _context.PrivateMessages
                        .Where(u => u.RecipientUsername == user.UserName && u.RecipientDeleted == false);
                    break;

                case "Outbox":
                    messageQuery = _context.PrivateMessages
                        .Where(u => u.SenderUsername == user.UserName && u.SenderDeleted == false);
                    break;

                default:
                    messageQuery = _context.PrivateMessages
                        .Where(u => u.RecipientUsername == user.UserName && u.DateRead == null);
                    break;
            }

            var messages = messageQuery
                .OrderByDescending(m => m.PrivateMessageSent)
                .Select(m => new MessageDto
                {
                    PrivateMessageId = m.PrivateMessageId,
                    SenderUsername = m.SenderUsername,
                    SenderProfilePictureUrl = m.Sender.ProfilePictureUrl,
                    RecipientUsername = m.RecipientUsername,
                    RecipientProfilePictureUrl = m.Recipient.ProfilePictureUrl,
                    Content = m.Content,
                    DateRead = m.DateRead.HasValue ? m.DateRead.Value.ToUniversalTime() : (DateTime?)null,
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
