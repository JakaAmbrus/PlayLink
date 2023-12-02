using MediatR;

namespace Application.Features.Users.GetUsersForSearchBar
{
    public class GetUsersForSearchBarQuery : IRequest<GetUsersForSearchBarResponse>
    {
        public int AuthUserId { get; set; }
    }
}
