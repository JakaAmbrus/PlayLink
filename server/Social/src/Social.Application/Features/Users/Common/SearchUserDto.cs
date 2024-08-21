namespace Social.Application.Features.Users.Common
{
    public class SearchUserDto
    {
        public int AppUserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Gender { get; set; }
    }
}
