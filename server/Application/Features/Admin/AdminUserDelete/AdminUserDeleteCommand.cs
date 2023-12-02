using MediatR;

namespace Application.Features.Admin.AdminUserDelete
{
    public class AdminUserDeleteCommand : IRequest<AdminUserDeleteResponse>
    {
        public int AppUserId { get; set; }
        public int AuthUserId { get; set; }
        public IEnumerable<string> AuthUserRoles { get; set; }
    }
}
