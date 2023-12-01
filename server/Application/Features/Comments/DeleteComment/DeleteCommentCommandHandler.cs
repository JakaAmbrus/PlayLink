using Application.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Comments.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, DeleteCommentResponse>
    {
        private readonly IApplicationDbContext  _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public DeleteCommentCommandHandler(IApplicationDbContext context,
            IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<DeleteCommentResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var selectedComment = await _context.Comments.FindAsync(request.CommentId, cancellationToken)
                ?? throw new NotFoundException("Comment was not found");

            var selectedPost = await _context.Posts.FindAsync(selectedComment.PostId, cancellationToken)
                ?? throw new NotFoundException("Post was not found");

            var authUserId = _authenticatedUserService.UserId;
            var authUserRole = _authenticatedUserService.UserRoles;

            bool isPostOwner = selectedComment.AppUserId == authUserId;
            bool isModerator = authUserRole.Contains("Moderator");

            //Only the comments owner or a moderator/admin can delete a comment
            if (!isPostOwner && !isModerator)
            {
                throw new UnauthorizedException("User not authorized to delete comment");
            }

            selectedPost.CommentsCount--;
            _context.Comments.Remove(selectedComment);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                throw new ServerErrorException("Problem deleting Commet");
            }

            return new DeleteCommentResponse { IsDeleted = true };
        }
    }
}
