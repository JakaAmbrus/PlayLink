using server.Extensions;

namespace server.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
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

    }
}
