using Social.Application.Features.Messages.Common;
using Social.Application.Utils;

namespace Social.Application.Features.Messages.GetMessagesForUser
{
    public class GetMessagesForUserResponse
    {
        public PagedList<MessageDto> Messages { get; set; }
    }
}
