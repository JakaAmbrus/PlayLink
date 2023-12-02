using Application.Features.Admin.Common;

namespace Application.Features.Admin.AdminGetUsers
{
    public class AdminGetUsersResponse
    {
        public List<UserWithRolesDto> Users { get; set; }
    }
}
