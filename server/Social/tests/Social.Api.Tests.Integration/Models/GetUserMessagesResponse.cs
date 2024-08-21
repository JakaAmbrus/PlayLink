using Social.Application.Features.Messages.Common;

namespace Social.Api.Tests.Integration.Models
{
    public class GetUserMessagesResponse
    {
        public List<MessageDto> Messages { get; set; }

        public GetUserMessagesResponse()
        {
            Messages = new List<MessageDto>();
        }
    }
}
