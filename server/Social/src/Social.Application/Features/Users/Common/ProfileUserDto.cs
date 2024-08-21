namespace Social.Application.Features.Users.Common
{
    public class ProfileUserDto
    {
        public int AppUserId { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Country { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public bool Authorized { get; set; }
    }
}
