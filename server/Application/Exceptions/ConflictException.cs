using System.Net;

namespace Application.Exceptions
{
    public class ConflictException : ApplicationExceptions
    {
        public ConflictException(string message)
            : base(HttpStatusCode.Conflict, message)
        {
        }
    }
}
