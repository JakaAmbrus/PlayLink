using System.Net;

namespace Social.Domain.Exceptions
{
    public class ConflictException : ApplicationExceptions
    {
        public ConflictException(string message)
            : base(HttpStatusCode.Conflict, message)
        {
        }
    }
}
