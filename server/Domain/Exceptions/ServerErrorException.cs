using System.Net;

namespace Domain.Exceptions
{
    public class ServerErrorException : ApplicationExceptions
    {
        public ServerErrorException(string message)
            : base(HttpStatusCode.InternalServerError, message)
        {
        }
    }
}
