using Application.Exceptions;
using Application.Features.Comments.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Comments.UploadComment
{
    public class UploadCommentCommandHandler : IRequestHandler<UploadCommentCommand, UploadCommentResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public UploadCommentCommandHandler(IApplicationDbContext context,
            IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }
        public async Task<UploadCommentResponse> Handle(UploadCommentCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FindAsync(request.PostId) 
                ?? throw new NotFoundException("Post not found");

            int currentUserId = _authenticatedUserService.UserId;
            var currentUser = await _context.Users.FindAsync(currentUserId, cancellationToken);
            if (currentUser == null)
            {
                throw new NotFoundException("User not found");
            }

            var newComment = new Comment
            {
                AppUserId = currentUserId,
                PostId = request.PostId,
                Content = request.CommentContentDto.Content
            };

            post.CommentsCount++;
            _context.Comments.Add(newComment);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                throw new ServerErrorException("Problem saving comment");
            }

            return new UploadCommentResponse
            {
                CommentDto = new CommentDto
                {
                    CommentId = newComment.CommentId,
                    PostId = newComment.PostId,
                    AppUserId = newComment.AppUserId,
                    Username = currentUser.UserName,
                    FullName = currentUser.FullName,
                    Gender = currentUser.Gender,
                    ProfilePictureUrl = currentUser.ProfilePictureUrl,
                    LikesCount = newComment.LikesCount,
                    IsAuthorized = true,
                    IsLikedByCurrentUser = false,
                    TimeCommented = newComment.TimeCommented,
                    Content = newComment.Content
                }
            };
            
        }
    }
}
