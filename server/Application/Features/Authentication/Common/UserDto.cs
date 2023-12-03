namespace Application.Features.Authentication.Common
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
