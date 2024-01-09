using Application.Features.Admin.Common;

namespace WebAPI.Tests.Integration.Models
{
    public class GetUsersWithRolesResponse
    {
        public List<UserWithRolesDto> Users { get; set; }

        public GetUsersWithRolesResponse()
        {
            Users = new List<UserWithRolesDto>();
        }
    }
}
