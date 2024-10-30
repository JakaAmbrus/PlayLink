using System.Net;

namespace Shield.Api.Common.Exceptions;

public class ServerErrorException : ApplicationException
{
    public ServerErrorException(string message) : base(HttpStatusCode.InternalServerError, message) { }
}