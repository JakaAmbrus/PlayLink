using System.Net;

namespace Domain.Exceptions
{
    public class NotFoundException : ApplicationExceptions
    {
        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound, message)
        {
        }
    }
}
