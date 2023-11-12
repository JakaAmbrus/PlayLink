using System.Net;

namespace Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public IDictionary<string, string[]> Errors { get; }

        public ApplicationException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public ApplicationException(string message, HttpStatusCode statusCode, IDictionary<string, string[]> errors)
            : base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}
