﻿using Application.Exceptions;
using Application.Features.Messages.Common;
using Application.Utils;
using Domain.Entities;
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

            var user = await _context.Users
                .FindAsync(new object[] { authUserId }, cancellationToken)
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
                    DateRead = m.DateRead,
                    PrivateMessageSent = m.PrivateMessageSent,
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
