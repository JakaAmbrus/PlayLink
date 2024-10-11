using Social.Application.Features.Admin.Common;

namespace Social.FunctionalTests.Models
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
