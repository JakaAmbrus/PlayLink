using Social.Domain.Enums;

namespace Social.Application.Utils
{
    public class MessageParams : PaginationParams
    {
        public string Container { get; set; } = MessageStatus.Unread.ToString();
    }
}
