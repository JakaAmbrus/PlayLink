using Social.Application.Features.Users.Common;
using Social.Application.Utils;

namespace Social.Application.Features.Users.GetUsers
{
    public class GetUsersResponse
    {
        public PagedList<UsersDto> Users { get; set; }
    }
}
