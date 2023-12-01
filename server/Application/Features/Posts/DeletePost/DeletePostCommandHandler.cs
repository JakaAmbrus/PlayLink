using Application.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Posts.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, DeletePostResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IPhotoService _photoService;

        public DeletePostCommandHandler(IApplicationDbContext context,
            IAuthenticatedUserService authenticatedUserService,
            IPhotoService photoService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
            _photoService = photoService;
        }

        public async Task<DeletePostResponse> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var selectedPost = await _context.Posts.FindAsync(request.PostId, cancellationToken) 
                ?? throw new NotFoundException("Post was not found");

            int authUserId = _authenticatedUserService.UserId;
            var authUserRole = _authenticatedUserService.UserRoles;

            bool isPostOwner = selectedPost.AppUserId == authUserId;
            bool isModerator = authUserRole.Contains("Moderator");

            //Only the posts owner or a moderator/admin can delete a post
            if (!isPostOwner && !isModerator)
            {
                throw new UnauthorizedException("User not authorized to delete post");
            }

            //Remove the photo from Cloudinary if it is included in the post
            if (!string.IsNullOrEmpty(selectedPost.PhotoPublicId))
            {
                var deletionResult = await _photoService.DeletePhotoAsync(selectedPost.PhotoPublicId);

                if (deletionResult.Error != null)
                {
                    throw new ServerErrorException(deletionResult.Error);
                }
            }

            _context.Posts.Remove(selectedPost);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                throw new ServerErrorException("Problem deleting post");
            }
            
            return new DeletePostResponse { IsDeleted = true };
        }
    }
}
