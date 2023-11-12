using System.Net;

namespace Application.Exceptions
{
    public class UnauthorizedException : ApplicationExceptions
    {
        public UnauthorizedException(string message)
            : base(HttpStatusCode.Unauthorized, message)
        {
        }
    }
}
