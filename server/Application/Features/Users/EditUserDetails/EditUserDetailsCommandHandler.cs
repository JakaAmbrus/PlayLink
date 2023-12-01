using Application.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Users.EditUserDetails
{
    public class EditUserDetailsCommandHandler : IRequestHandler<EditUserDetailsCommand, EditUserDetailsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IPhotoService _photoService;

        public EditUserDetailsCommandHandler(IApplicationDbContext context,
                       IAuthenticatedUserService authenticatedUserService,
                                  IPhotoService photoService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
            _photoService = photoService;
        }

        public async Task<EditUserDetailsResponse> Handle(EditUserDetailsCommand request, CancellationToken cancellationToken)
        {
            var authUserId = _authenticatedUserService.UserId;

            var selectedUser = await _context.Users.FindAsync(authUserId, cancellationToken)
                ?? throw new NotFoundException("User was not found");

            var username = selectedUser.UserName;
            var authUserRole = _authenticatedUserService.UserRoles;

            bool isModerator = authUserRole.Contains("Moderator");
            bool isUserOwner = username == request.EditUserDto.Username;

            //Only the owner or a moderator/admin can edit post
            if (!isUserOwner && !isModerator)
            {
                throw new UnauthorizedException("User not authorized to edit profile");
            }

            //Add Photo to user and store it in cloudinary
            if (request.EditUserDto.PhotoFile != null && request.EditUserDto.PhotoFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(selectedUser.ProfilePicturePublicId))
                {
                    var deletionResult = await _photoService.DeletePhotoAsync(selectedUser.ProfilePicturePublicId);

                    if (deletionResult.Error != null)
                    {
                        throw new ServerErrorException(deletionResult.Error);
                    }
                }
                var uploadResult = await _photoService.AddPhotoAsync(request.EditUserDto.PhotoFile, "profile");

                if (uploadResult.Error == null)
                {
                    selectedUser.ProfilePictureUrl = uploadResult.Url.ToString();
                    selectedUser.ProfilePicturePublicId = uploadResult.PublicId.ToString();
                }
                else
                {
                    throw new ServerErrorException(uploadResult.Error);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.EditUserDto.Description))
            {
                selectedUser.Description = request.EditUserDto.Description;
            }

            if (!string.IsNullOrWhiteSpace(request.EditUserDto.Country))
            {
                selectedUser.Country = request.EditUserDto.Country;
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                throw new ServerErrorException("Problem editing profile");
            }

            return new EditUserDetailsResponse
            {
                PhotoUrl = selectedUser.ProfilePictureUrl,
                Description = selectedUser.Description,
                Country = selectedUser.Country,
            };
        }       
    }
}
