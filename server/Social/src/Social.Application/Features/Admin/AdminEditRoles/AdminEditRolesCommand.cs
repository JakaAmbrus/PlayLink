using MediatR;

namespace Social.Application.Features.Admin.AdminEditRoles
{
    public class AdminEditRolesCommand : IRequest<AdminEditRolesResponse>
    {
        public int AppUserId { get; set; }
        public int AuthUserId { get; set; }
    }
}
