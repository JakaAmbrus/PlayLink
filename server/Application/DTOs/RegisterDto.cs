using Application.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength=4)]
        public string Password { get; set; }
        [Required]
        [FullName(ErrorMessage = "Full name must contain at least a first and a last name separated by a space")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "FullName can only contain letters and spaces.")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "The input has to be between 2 and 15 characters")]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Country can only contain letters and spaces.")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "The input has to be between 2 and 15 characters")]
        public string Country { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "The input has to be between 2 and 15 characters")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "City can only contain letters and spaces.")]
        public string City { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }

    }
}
