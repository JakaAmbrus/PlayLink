using Application.Exceptions;
using Application.Features.Messages.Common;
using Application.Utils;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Features.Messages.GetMessagesForUser
{
    public class GetMessagesForUserQueryHandler : IRequestHandler<GetMessagesForUserQuery, GetMessagesForUserResponse>
    {
        private readonly DataContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public GetMessagesForUserQueryHandler(DataContext context,
                                  IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<GetMessagesForUserResponse> Handle(GetMessagesForUserQuery request, CancellationToken cancellationToken)
        {
            int authUserId = _authenticatedUserService.UserId;

            var user = await _context.Users.FindAsync(authUserId, cancellationToken)
                ?? throw new NotFoundException("User not found");

            var messages = _context.PrivateMessages
                .AsQueryable()
                .OrderByDescending(m => m.PrivateMessageSent)
                .Select(m => new MessageDto
                {
                    PrivateMessageId = m.PrivateMessageId,
                    SenderId = m.Sender.Id,
                    SenderUsername = m.SenderUsername,
                    SenderProfilePictureUrl = m.Sender.ProfilePictureUrl,
                    RecipientId = m.Recipient.Id,
                    RecipientUsername = m.RecipientUsername,
                    RecipientProfilePictureUrl = m.Recipient.ProfilePictureUrl,
                    Content = m.Content,
                    DateRead = m.DateRead,
                    PrivateMessageSent = m.PrivateMessageSent
                });

            messages = request.Params.Container switch
            {
                "Inbox" => messages.Where(u => u.RecipientUsername == user.UserName),
                "Outbox" => messages.Where(u => u.SenderUsername == user.UserName),
                _ => messages.Where(u => u.RecipientUsername == user.UserName && u.DateRead == null)
            };

            var pagedMessages = await PagedList<MessageDto>
                .CreateAsync(messages, request.Params.PageNumber, request.Params.PageSize);

            return new GetMessagesForUserResponse { Messages = pagedMessages };
        }
    }
}
