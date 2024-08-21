using System.Net;

namespace Social.Domain.Exceptions
{
    public class NotFoundException : ApplicationExceptions
    {
        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound, message)
        {
        }
    }
}
