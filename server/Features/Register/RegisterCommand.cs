using MediatR;
using server.Attributes;
using System.ComponentModel.DataAnnotations;

namespace server.Features.Register
{
    public class RegisterCommand : IRequest<RegisterResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
