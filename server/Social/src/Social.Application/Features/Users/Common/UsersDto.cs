namespace Social.Application.Features.Users.Common
{
    public class UsersDto
    {
        public int AppUserId { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
