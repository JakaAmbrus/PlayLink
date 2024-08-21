using MediatR;
using Social.Application.Utils;

namespace Social.Application.Features.Admin.AdminGetUsers
{
    public class AdminGetUsersQuery : IRequest<AdminGetUsersResponse>
    {
        public PaginationParams Params { get; set; } = new PaginationParams();
        public int AuthUserId { get; set; }
    }
}
