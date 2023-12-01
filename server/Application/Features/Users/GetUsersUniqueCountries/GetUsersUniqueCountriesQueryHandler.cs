using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.GetUsersUniqueCountries
{
    public class GetUsersUniqueCountriesQueryHandler : IRequestHandler<GetUsersUniqueCountriesQuery, GetUsersUniqueCountriesResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public GetUsersUniqueCountriesQueryHandler(IApplicationDbContext context, IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<GetUsersUniqueCountriesResponse> Handle(GetUsersUniqueCountriesQuery request, CancellationToken cancellationToken)
        {
            int authUserId = _authenticatedUserService.UserId;

            var countries = await _context.Users
                .AsQueryable()
                .Where(u => u.Id != authUserId) //I do not want to receive the users country since they will not be in Discover section
                .Select(u => u.Country)
                .Distinct()
                .ToListAsync(cancellationToken);

            if (countries == null)
            {
                throw new NotFoundException("Countries not found");
            }   

            return new GetUsersUniqueCountriesResponse
            {
                Countries = countries
            };
        }
    }
}
