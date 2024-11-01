using Social.Domain.Exceptions;
using MediatR;
using Social.Application.Interfaces;

namespace Social.Application.Features.Users.EditUserDetails
{
    public class EditUserDetailsCommandHandler : IRequestHandler<EditUserDetailsCommand, EditUserDetailsResult>
    {
        private readonly ISocialDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public EditUserDetailsCommandHandler(ISocialDbContext context,IPhotoService photoService, ICacheInvalidationService cacheInvalidationService)
        {
            _context = context;
            _photoService = photoService;
            _cacheInvalidationService = cacheInvalidationService;
        }

        public async Task<EditUserDetailsResult> Handle(EditUserDetailsCommand request, CancellationToken cancellationToken)
        {
            var authUser = await _context.Users.FindAsync(request.AuthUserId)
                ?? throw new NotFoundException("Authorized user was not found");

            bool isUserOwner = authUser.Username == request.EditUserDto.Username;

            if (!isUserOwner)
            {
                throw new UnauthorizedException("User not authorized to edit profile");
            }
            
            if (request.EditUserDto.PhotoFile != null && request.EditUserDto.PhotoFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(authUser.ProfilePicturePublicId))
                {
                    var deletionResult = await _photoService.DeletePhotoAsync(authUser.ProfilePicturePublicId);

                    if (deletionResult.Error != null)
                    {
                        throw new ServerErrorException(deletionResult.Error);
                    }
                }
                var uploadResult = await _photoService.AddPhotoAsync(request.EditUserDto.PhotoFile, "profile");

                if (uploadResult.Error == null)
                {
                    authUser.ProfilePictureUrl = uploadResult.Url.ToString();
                    authUser.ProfilePicturePublicId = uploadResult.PublicId.ToString();

                    _cacheInvalidationService.InvalidateUserPhotosCache(authUser.Username);
                }
                else
                {
                    throw new ServerErrorException(uploadResult.Error);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.EditUserDto.Description))
            {
                authUser.Description = request.EditUserDto.Description;
            }

            if (!string.IsNullOrWhiteSpace(request.EditUserDto.Country))
            {
                authUser.Country = request.EditUserDto.Country;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new EditUserDetailsResult
            {
                PhotoUrl = authUser.ProfilePictureUrl,
                Description = authUser.Description,
                Country = authUser.Country,
            };
        }       
    }
}
