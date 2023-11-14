using Application.Exceptions;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Features.Posts.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, DeletePostResponse>
    {
        private readonly DataContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public DeletePostCommandHandler(DataContext context, IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<DeletePostResponse> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var selectedPost = await _context.Posts.FindAsync(request.PostId, cancellationToken) 
                ?? throw new NotFoundException("Post was not found");

            var authUserId = _authenticatedUserService.UserId;
            var authUserRole = _authenticatedUserService.UserRoles;

            bool isPostOwner = selectedPost.AppUserId == authUserId;
            bool isModerator = authUserRole.Contains("Moderator");

            //Only the posts owner or a moderator/admin can delete a post
            if (!isPostOwner && !isModerator)
            {
                throw new UnauthorizedException("User not authorized to delete post");
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
