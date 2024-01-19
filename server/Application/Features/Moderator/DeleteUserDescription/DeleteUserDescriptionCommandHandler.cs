using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Moderator.DeleteUserDescription
{
    public class DeleteUserDescriptionCommandHandler : IRequestHandler<DeleteUserDescriptionCommand, DeleteUserDescriptionResponse>
    {
        private readonly IApplicationDbContext _context;

        public DeleteUserDescriptionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteUserDescriptionResponse> Handle(DeleteUserDescriptionCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken)
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