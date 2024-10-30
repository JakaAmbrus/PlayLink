using System.Net;

namespace Shield.Api.Common.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message)
        {
        }
    }
}
