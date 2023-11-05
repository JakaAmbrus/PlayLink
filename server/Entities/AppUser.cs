using Microsoft.AspNetCore.Identity;
using server.Extensions;

namespace server.Entities
{
    public class AppUser : IdentityUser<int>
    {
        
  /*      public string FullName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string ProfileDescription { get; set; }
        public string ProfilePictureUrl { get; set; }

        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }*/

        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
