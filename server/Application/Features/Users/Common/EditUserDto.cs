using Microsoft.AspNetCore.Http;

namespace Application.Features.Users.Common
{
    public class EditUserDto
    {
        public string Username { get; set; }
        public IFormFile PhotoFile { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }

    }
}
