using FluentValidation.Results;
using System.Net;

namespace Application.Exceptions
{
    public class ValidationException : ApplicationExceptions
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base(HttpStatusCode.BadRequest, "One or more validation errors occurred.")
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}

