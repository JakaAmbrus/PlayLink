using Application.Utils;
using MediatR;

namespace Application.Features.Users.GetUsers
{
    public class GetUsersQuery : IRequest<GetUsersResponse>
    {
       public UserParams Params { get; set; } = new UserParams();
    }
}
