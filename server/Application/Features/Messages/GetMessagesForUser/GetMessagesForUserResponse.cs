using Application.Features.Messages.Common;
using Application.Utils;

namespace Application.Features.Messages.GetMessagesForUser
{
    public class GetMessagesForUserResponse
    {
        public PagedList<MessageDto> Messages { get; set; }
    }
}
