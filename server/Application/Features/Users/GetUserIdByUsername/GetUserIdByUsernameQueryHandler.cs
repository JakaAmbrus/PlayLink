using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.GetUserIdFromUsername
{
    public class GetUserIdByUsernameQueryHandler : IRequestHandler<GetUserIdByUsernameQuery, int>
    {
        private readonly IApplicationDbContext _context;

        public GetUserIdByUsernameQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(GetUserIdByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken) 
                ?? throw new NotFoundException("User not found");

            return user.Id;
        }
    }
}
