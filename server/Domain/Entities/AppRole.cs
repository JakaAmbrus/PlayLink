using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
