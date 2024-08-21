using MediatR;
using Social.Application.Utils;

namespace Social.Application.Features.Users.GetUsers
{
    public class GetUsersQuery : IRequest<GetUsersResponse>
    {
       public UserParams Params { get; set; } = new UserParams();
       public int AuthUserId { get; set; }
    }
}
