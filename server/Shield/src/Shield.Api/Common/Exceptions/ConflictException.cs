using System.Net;

namespace Shield.Api.Common.Exceptions
{
    public class ConflictException : ApplicationException
    {
        public ConflictException(string message) : base(HttpStatusCode.Conflict, message)
        {
        }
    }
}
