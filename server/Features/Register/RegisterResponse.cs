﻿using server.Features.Common;

namespace server.Features.Register
{
    public class RegisterResponse
    {
        public UserDto User { get; set; }
        public List<string> Errors { get; set; }
        public bool Success { get; set; }
    }
}
