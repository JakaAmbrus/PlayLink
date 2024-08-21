using MediatR;

namespace Social.Application.Features.Authentication.UserRegistration
{
    public class UserRegistrationCommand : IRequest<UserRegistrationResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
