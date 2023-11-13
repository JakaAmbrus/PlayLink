using System.Net;

namespace Application.Exceptions
{
    public class BadRequestException : ApplicationExceptions
    {
        public BadRequestException(string message)
            : base(HttpStatusCode.BadRequest, message)
        {
        }
    }
}
