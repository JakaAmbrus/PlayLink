using Application.Features.Authentication.Common;

namespace Application.Features.Authentication.UserLogin
{
    public class UserLoginResponse
    {
        public UserDto User { get; set; }
        public List<string> Errors { get; set; }
        public bool Success { get; set; }
    }
}
