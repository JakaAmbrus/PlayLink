using Application.Exceptions;
using Application.Features.Messages.Common;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Messages.GetMessageById
{
    public class GetMessageByIdQueryHandler : IRequestHandler<GetMessageByIdQuery, MessageDto>
    {
        private readonly IApplicationDbContext _context;

        public GetMessageByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MessageDto> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
        {
            var message = await _context.PrivateMessages.FindAsync(new object[] { request.MessageId }, cancellationToken)
                ?? throw new NotFoundException("Message not found");

            return new MessageDto
            {
                PrivateMessageId = message.PrivateMessageId,
                SenderUsername = message.Sender.UserName,
                RecipientUsername = message.Recipient.UserName,
                SenderFullName = message.Sender.FullName,
                RecipientFullName = message.Recipient.FullName,
                SenderGender = message.Sender.Gender,
                RecipientGender = message.Sender.Gender,
                SenderProfilePictureUrl = message.Sender.ProfilePictureUrl,
                RecipientProfilePictureUrl = message.Recipient.ProfilePictureUrl,
                DateRead = message.DateRead,
                PrivateMessageSent = message.PrivateMessageSent,
                Content = message.Content,
            };            
        }
    }
}
