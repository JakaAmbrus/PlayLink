using Application.Features.Admin.Common;
using Application.Utils;

namespace Application.Features.Admin.AdminGetUsers
{
    public class AdminGetUsersResponse
    {
        public PagedList<UserWithRolesDto> Users { get; set; }
    }
}
