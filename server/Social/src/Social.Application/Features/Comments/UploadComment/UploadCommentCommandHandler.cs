using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;
using Social.Application.Features.Comments.Common;
using Social.Application.Interfaces;

namespace Social.Application.Features.Comments.UploadComment
{
    public class UploadCommentCommandHandler : IRequestHandler<UploadCommentCommand, UploadCommentResponse>
    {
        private readonly ISocialDbContext _context;

        public UploadCommentCommandHandler(ISocialDbContext context)
        {
            _context = context;
        }
        public async Task<UploadCommentResponse> Handle(UploadCommentCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FindAsync(request.Comment.PostId)
                ?? throw new NotFoundException("Post not found");

            var authUser = await _context.Users.FindAsync(request.AuthUserId) 
                ?? throw new NotFoundException("User not found");

            var newComment = new Comment
            {
                AppUserId = request.AuthUserId,
                PostId = request.Comment.PostId,
                Content = request.Comment.Content
            };

            post.CommentsCount++;
            _context.Comments.Add(newComment);

            await _context.SaveChangesAsync(cancellationToken);

            return new UploadCommentResponse
            {
                CommentDto = new CommentDto
                {
                    CommentId = newComment.CommentId,
                    PostId = newComment.PostId,
                    AppUserId = newComment.AppUserId,
                    Username = authUser.Username,
                    FullName = authUser.FullName,
                    Gender = authUser.Gender,
                    ProfilePictureUrl = authUser.ProfilePictureUrl,
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
