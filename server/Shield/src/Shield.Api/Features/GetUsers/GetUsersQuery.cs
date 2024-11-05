using MediatR;

namespace Shield.Api.Features.GetUsers;

public class GetUsersQuery : IRequest<GetUsersResponse>
{
    public int PagedLimit { get; set; }
}