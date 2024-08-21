using MediatR;
using Social.Application.Features.Messages.Common;

namespace Social.Application.Features.Messages.GetMessageById
{
    public class GetMessageByIdQuery : IRequest<MessageDto>
    {
        public int MessageId { get; set; }
    }
}
