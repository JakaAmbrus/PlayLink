using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Features.Messages.Common;
using Social.Application.Interfaces;

namespace Social.Application.Features.Messages.SendMessage
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, SendMessageResponse>
    {
        private readonly ISocialDbContext _context;

        public SendMessageCommandHandler(ISocialDbContext context)
        {
            _context = context;
        }

        public async Task<SendMessageResponse> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var sender = await _context.Users.FindAsync(request.AuthUserId)
                ?? throw new NotFoundException("Sender not found");

            var recipient = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.CreateMessageDto.RecipientUsername, cancellationToken)
                ?? throw new NotFoundException("Recipient not found");


            if (recipient.Username == sender.Username)
            {
                throw new BadRequestException("You cannot send messages to yourself");
            }

            var message = new PrivateMessage
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.Username,
                RecipientUsername = recipient.Username,
                Content = request.CreateMessageDto.Content
            };

            _context.PrivateMessages.Add(message);
            await _context.SaveChangesAsync(cancellationToken);

            return new SendMessageResponse
            {
                Message = new MessageDto
                {
                    PrivateMessageId = message.PrivateMessageId,
                    SenderFullName = message.Sender.FullName,
                    SenderGender = message.Sender.Gender,
                    SenderUsername = message.SenderUsername,
                    RecipientUsername = message.RecipientUsername,
                    RecipientFullName = message.Recipient.FullName,
                    RecipientGender = message.Recipient.Gender,
                    Content = message.Content,
                    DateRead = message.DateRead?.ToUniversalTime(),
                    PrivateMessageSent = message.PrivateMessageSent.ToUniversalTime(),
                    SenderProfilePictureUrl = message.Sender.ProfilePictureUrl,
                    RecipientProfilePictureUrl = message.Recipient.ProfilePictureUrl    
                }
            };
        }   
    }
}
