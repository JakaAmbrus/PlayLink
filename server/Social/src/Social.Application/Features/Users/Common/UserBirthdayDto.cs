namespace Social.Application.Features.Users.Common
{
    public class UserBirthdayDto
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int DaysUntilBirthday { get; set; }
    }
}
