using Application.Utils;
using MediatR;

namespace Application.Features.Users.GetUsers
{
    public class GetUsersQuery : IRequest<GetUsersResponse>
    {
       public PaginationParams PaginationParams { get; set; } = new PaginationParams();
    }
}
