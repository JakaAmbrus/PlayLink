using System.Net;

namespace Shield.Api.Common.Exceptions;

public sealed class ValidationException : ApplicationException
{
    public IReadOnlyDictionary<string, string[]> ValidationErrors { get; }

    public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary)
        : base(HttpStatusCode.BadRequest, "Validation Errors Occurred")
    {
        ValidationErrors = errorsDictionary;
    }
}