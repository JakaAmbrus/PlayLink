using Application.Features.Common;

namespace Application.Features.Register
{
    public class RegisterResponse
    {
        public UserDto User { get; set; }
        public List<string> Errors { get; set; }
        public bool Success { get; set; }
    }
}
