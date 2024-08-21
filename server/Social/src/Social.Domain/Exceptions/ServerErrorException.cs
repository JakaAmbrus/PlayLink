using System.Net;

namespace Social.Domain.Exceptions
{
    public class ServerErrorException : ApplicationExceptions
    {
        public ServerErrorException(string message)
            : base(HttpStatusCode.InternalServerError, message)
        {
        }
    }
}
