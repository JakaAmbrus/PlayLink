using MediatR;

namespace Application.Features.Authentication.UserRegistration
{
    public class UserRegistrationCommand : IRequest<UserRegistrationResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
