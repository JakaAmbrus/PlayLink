using MediatR;
using Application.Attributes;
using System.ComponentModel.DataAnnotations;
using Application.Features.Register;

namespace Application.Features.Register
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
