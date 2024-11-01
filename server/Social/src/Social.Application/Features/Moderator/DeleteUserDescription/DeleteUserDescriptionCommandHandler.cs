using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Interfaces;

namespace Social.Application.Features.Moderator.DeleteUserDescription
{
    public class DeleteUserDescriptionCommandHandler : IRequestHandler<DeleteUserDescriptionCommand, DeleteUserDescriptionResponse>
    {
        private readonly ISocialDbContext _context;

        public DeleteUserDescriptionCommandHandler(ISocialDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteUserDescriptionResponse> Handle(DeleteUserDescriptionCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken)
                ?? throw new NotFoundException("User not found.");

            if (user.Description == null)
            {
                return new DeleteUserDescriptionResponse { IsDeleted = false };
            }

            user.Description = null;

            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteUserDescriptionResponse { IsDeleted = true };
        }
    }
}