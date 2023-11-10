using MediatR;

namespace Application.Features.Authentication.UserLogin
{
    public class UserLoginCommand : IRequest<UserLoginResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
