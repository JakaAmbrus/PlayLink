using Application.Features.Messages.Common;

namespace WebAPI.Tests.Integration.Models
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
