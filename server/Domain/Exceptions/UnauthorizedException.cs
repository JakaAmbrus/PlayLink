﻿using System.Net;

namespace Domain.Exceptions
{
    public class UnauthorizedException : ApplicationExceptions
    {
        public UnauthorizedException(string message)
            : base(HttpStatusCode.Unauthorized, message)
        {
        }
    }
}
