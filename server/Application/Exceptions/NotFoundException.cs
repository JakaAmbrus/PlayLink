using System.Net;

namespace Application.Exceptions
{
    public class NotFoundException : ApplicationExceptions
    {
        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound, message)
        {
        }
    }
}
