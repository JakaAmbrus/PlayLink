using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<AppUser>>
    {
        private readonly DataContext _context;

        public GetUsersQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<List<AppUser>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
         if (_context.Users == null)
            {
                return null;
            }

            return await _context.Users.ToListAsync(cancellationToken);
        }
    }
}
