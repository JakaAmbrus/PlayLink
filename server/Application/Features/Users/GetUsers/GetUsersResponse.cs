using Application.Features.Users.Common;
using Application.Utils;

namespace Application.Features.Users.GetUsers
{
    public class GetUsersResponse
    {
        public PagedList<UsersDto> Users { get; set; }
    }
}
