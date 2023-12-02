using MediatR;

namespace Application.Features.Admin.AdminEditRoles
{
    public class AdminEditRolesCommand : IRequest<AdminEditRolesResponse>
    {
        public int AppUserId { get; set; }
        public bool AssignModeratorRole { get; set; }
        public int AuthUserId { get; set; }
    }
}
