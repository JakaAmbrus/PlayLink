using MediatR;

namespace Application.Features.Users.GetUsersUniqueCountries
{
    public class GetUsersUniqueCountriesQuery : IRequest<GetUsersUniqueCountriesResponse>
    {
        public int AuthUserId { get; set; }
    }
}
