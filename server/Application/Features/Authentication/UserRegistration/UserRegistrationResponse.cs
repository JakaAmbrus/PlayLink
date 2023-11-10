using Application.Features.Authentication.Common;

namespace Application.Features.Authentication.UserRegistration
{
    public class UserRegistrationResponse
    {
        public UserDto User { get; set; }
        public List<string> Errors { get; set; }
        public bool Success { get; set; }
    }
}
