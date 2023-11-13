using System.Net;

namespace Application.Exceptions
{
    internal class ServerErrorException : ApplicationExceptions
    {
        public ServerErrorException(string message)
            : base(HttpStatusCode.InternalServerError, message)
        {
        }
    }
}
