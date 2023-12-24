using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Moderator.DeleteUserDescription
{
    public class DeleteUserDescriptionCommandHandler : IRequestHandler<DeleteUserDescriptionCommand, DeleteUserDescriptionResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public DeleteUserDescriptionCommandHandler(IApplicationDbContext context, ICacheInvalidationService cacheInvalidationService)
        {
            _context = context;
            _cacheInvalidationService = cacheInvalidationService;
        }

        public async Task<DeleteUserDescriptionResponse> Handle(DeleteUserDescriptionCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken)
                ?? throw new NotFoundException("User not found.");

            user.Description = null;

            await _context.SaveChangesAsync(cancellationToken);

            _cacheInvalidationService.InvalidateUserCache(user.UserName);

            return new DeleteUserDescriptionResponse { IsDeleted = true };
        }
    }
}