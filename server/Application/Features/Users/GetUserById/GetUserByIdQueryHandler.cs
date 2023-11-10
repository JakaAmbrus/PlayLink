using Domain.Entities;
using Infrastructure.Data;
using MediatR;

namespace Application.Features.Users.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, AppUser>
    {
        private readonly DataContext _context;

        public GetUserByIdQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<AppUser> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken);
            return user;
        }
    }
}
