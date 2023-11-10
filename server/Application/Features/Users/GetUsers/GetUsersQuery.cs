using Domain.Entities;
using MediatR;

namespace Application.Features.Users.GetUsers
{
    public class GetUsersQuery : IRequest<List<AppUser>>
    {
    }
}
