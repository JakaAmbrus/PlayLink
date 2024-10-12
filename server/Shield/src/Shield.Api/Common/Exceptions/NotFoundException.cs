using System.Net;

namespace Shield.Api.Common.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base(HttpStatusCode.NotFound, message) { }
    }
}
