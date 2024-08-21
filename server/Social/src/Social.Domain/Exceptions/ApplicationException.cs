using System.Net;

namespace Social.Domain.Exceptions
{
    public class ApplicationExceptions : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public IDictionary<string, string[]> Errors { get; }

        public ApplicationExceptions(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public ApplicationExceptions(HttpStatusCode statusCode, string message, IDictionary<string, string[]> errors)
            : base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}
