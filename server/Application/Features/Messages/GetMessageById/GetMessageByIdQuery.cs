using Application.Features.Messages.Common;
using MediatR;

namespace Application.Features.Messages.GetMessageById
{
    public class GetMessageByIdQuery : IRequest<MessageDto>
    {
        public int MessageId { get; set; }
    }
}
