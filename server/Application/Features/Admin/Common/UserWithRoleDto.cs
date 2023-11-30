namespace Application.Features.Admin.Common
{
    public class UserWithRoleDto
    {
        public int AppUserId { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
