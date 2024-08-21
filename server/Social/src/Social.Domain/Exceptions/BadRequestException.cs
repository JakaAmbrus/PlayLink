using System.Net;

namespace Social.Domain.Exceptions
{
    public class BadRequestException : ApplicationExceptions
    {
        public BadRequestException(string message)
            : base(HttpStatusCode.BadRequest, message)
        {
        }
    }
}
