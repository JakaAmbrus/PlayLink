using MediatR;
using Social.Application.Features.Users.Common;

namespace Social.Application.Features.Users.EditUserDetails
{
    public class EditUserDetailsCommand : IRequest<EditUserDetailsResult>
    {
        public EditUserDto EditUserDto { get; set; }
        public int AuthUserId { get; set; }
        public IEnumerable<string> AuthUserRoles { get; set; }
    }
}
