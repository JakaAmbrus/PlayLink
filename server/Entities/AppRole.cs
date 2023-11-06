using Microsoft.AspNetCore.Identity;

namespace server.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public List<AppUserRole> UserRoles { get; set; }
    }
}
