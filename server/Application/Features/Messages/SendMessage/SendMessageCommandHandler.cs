using Application.Exceptions;
using Application.Features.Messages.Common;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Messages.SendMessage
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, SendMessageResponse>
    {
        private readonly DataContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public SendMessageCommandHandler(DataContext context,
                       IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<SendMessageResponse> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {

            int senderId = _authenticatedUserService.UserId;

            var sender = await _context.Users.FindAsync(senderId, cancellationToken)
                ?? throw new NotFoundException("User not found");

            var recipient = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.CreateMessageDto.RecipientUsername, cancellationToken)
                ?? throw new NotFoundException("Recipient not found");


            if (recipient.UserName == sender.UserName)
            {
                throw new BadRequestException("You cannot send messages to yourself");
            }

            var message = new PrivateMessage
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = request.CreateMessageDto.Content
            };

            _context.Add(message);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ServerErrorException(ex.Message);
            }

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
                    DateRead = message.DateRead,
                    PrivateMessageSent = message.PrivateMessageSent,
                    SenderProfilePictureUrl = message.Sender.ProfilePictureUrl,
                    RecipientProfilePictureUrl = message.Recipient.ProfilePictureUrl    
                }
            };
        }   
    }
}
