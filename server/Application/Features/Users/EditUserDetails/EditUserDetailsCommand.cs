using Application.Features.Users.Common;
using MediatR;

namespace Application.Features.Users.EditUserDetails
{
    public class EditUserDetailsCommand : IRequest<EditUserDetailsResponse>
    {
        public EditUserDto EditUserDto { get; set; }
    }
}
