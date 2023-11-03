using System.ComponentModel.DataAnnotations;

namespace server.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength=4)]
        public string Password { get; set; }
    }
}
