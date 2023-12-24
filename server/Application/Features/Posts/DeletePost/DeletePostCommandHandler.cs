using Application.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Posts.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, DeletePostResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public DeletePostCommandHandler(IApplicationDbContext context, IPhotoService photoService, ICacheInvalidationService cacheInvalidationService)
        {
            _context = context;
            _photoService = photoService;
            _cacheInvalidationService = cacheInvalidationService;
        }

        public async Task<DeletePostResponse> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var authUser = await _context.Users.FindAsync(new object[] { request.AuthUserId }, cancellationToken)
                        ?? throw new NotFoundException("Authorized user not found");

            var selectedPost = await _context.Posts.FindAsync(new object[] { request.PostId }, cancellationToken)
                ?? throw new NotFoundException("Post was not found");

            bool isPostOwner = selectedPost.AppUserId == request.AuthUserId;
            bool isModerator = request.AuthUserRoles.Contains("Moderator");

            //Only the posts owner or a moderator/admin can delete a post
            if (!isPostOwner && !isModerator)
            {
                throw new UnauthorizedException("User not authorized to delete post");
            }

            //Remove the photo from Cloudinary if it is included in the post
            if (!string.IsNullOrEmpty(selectedPost.PhotoPublicId))
            {
                var deletionResult = await _photoService.DeletePhotoAsync(selectedPost.PhotoPublicId);

                if (deletionResult.Error == null) 
                {
                    _cacheInvalidationService.InvalidateUserPhotosCache(authUser.UserName);
                }

                if (deletionResult.Error != null)
                {
                    throw new ServerErrorException(deletionResult.Error);
                }
            }

            _context.Posts.Remove(selectedPost);
            await _context.SaveChangesAsync(cancellationToken);
 
            return new DeletePostResponse { IsDeleted = true };
        }
    }
}
