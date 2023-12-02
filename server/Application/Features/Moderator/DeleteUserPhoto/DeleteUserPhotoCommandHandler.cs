using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Moderator.DeleteUserPhoto
{
    public class DeleteUserPhotoCommandHandler : IRequestHandler<DeleteUserPhotoCommand, DeleteUserPhotoResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPhotoService _photoService;

        public DeleteUserPhotoCommandHandler(IApplicationDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        public async Task<DeleteUserPhotoResponse> Handle(DeleteUserPhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken)
                ?? throw new NotFoundException("User not found.");

            if (string.IsNullOrEmpty(user.ProfilePicturePublicId))
            {
                throw new NotFoundException("User does not have a profile picture.");
            }

            var deletionResult = await _photoService.DeletePhotoAsync(user.ProfilePicturePublicId);

            if (deletionResult.Error != null)
            {
                throw new ServerErrorException(deletionResult.Error);
            }

            user.ProfilePictureUrl = null;
            user.ProfilePicturePublicId = null;

            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteUserPhotoResponse { IsDeleted = true };
        }
    }
}