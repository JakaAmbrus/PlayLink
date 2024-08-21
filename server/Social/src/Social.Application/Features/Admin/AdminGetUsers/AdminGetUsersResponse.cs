using Social.Application.Features.Admin.Common;
using Social.Application.Utils;

namespace Social.Application.Features.Admin.AdminGetUsers
{
    public class AdminGetUsersResponse
    {
        public PagedList<UserWithRolesDto> Users { get; set; }
    }
}
