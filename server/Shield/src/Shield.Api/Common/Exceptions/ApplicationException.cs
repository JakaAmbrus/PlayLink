using System.Net;

namespace Shield.Api.Common.Exceptions;

public class ApplicationException : Exception
{
    public HttpStatusCode StatusCode { get; }

    protected ApplicationException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}