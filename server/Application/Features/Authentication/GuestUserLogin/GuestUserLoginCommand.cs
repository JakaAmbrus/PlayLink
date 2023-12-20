using MediatR;

namespace Application.Features.Authentication.GuestUserLogin
{
    public class GuestUserLoginCommand : IRequest<GuestUserLoginResponse>
    {
        public string Role { get; set; }
    }
}
