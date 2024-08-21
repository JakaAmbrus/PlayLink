using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Interfaces;

namespace Social.Application.Features.Users.GetUsersUniqueCountries
{
    public class GetUsersUniqueCountriesQueryHandler : IRequestHandler<GetUsersUniqueCountriesQuery, GetUsersUniqueCountriesResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetUsersUniqueCountriesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetUsersUniqueCountriesResponse> Handle(GetUsersUniqueCountriesQuery request, CancellationToken cancellationToken)
        {
            var countries = await _context.Users
                .AsQueryable()
                .Where(u => u.Id != request.AuthUserId) //I do not want to receive the users country since they will not be in Discover section
                .Select(u => u.Country)
                .Distinct()
                .ToListAsync(cancellationToken);

            if (countries == null)
            {
                return new GetUsersUniqueCountriesResponse { Countries = new List<string>() };
            }

            return new GetUsersUniqueCountriesResponse { Countries = countries };
        }
    }
}
