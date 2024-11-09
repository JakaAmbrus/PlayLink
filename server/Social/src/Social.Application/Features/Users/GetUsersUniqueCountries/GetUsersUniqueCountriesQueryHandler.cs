using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Interfaces;

namespace Social.Application.Features.Users.GetUsersUniqueCountries
{
    public class GetUsersUniqueCountriesQueryHandler : IRequestHandler<GetUsersUniqueCountriesQuery, GetUsersUniqueCountriesResponse>
    {
        private readonly ISocialDbContext _context;

        public GetUsersUniqueCountriesQueryHandler(ISocialDbContext context)
        {
            _context = context;
        }

        public async Task<GetUsersUniqueCountriesResponse> Handle(GetUsersUniqueCountriesQuery request, CancellationToken cancellationToken)
        {
            var countries = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id != request.AuthUserId) //I do not want to receive the users country since they will not be in Discover section
                .Select(u => u.Country)
                .Distinct()
                .ToListAsync(cancellationToken);

            return new GetUsersUniqueCountriesResponse { Countries = countries };
        }
    }
}
