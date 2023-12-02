namespace Application.Features.Admin.Common
{
    public class UserWithRolesDto
    {
        public int AppUserId { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
