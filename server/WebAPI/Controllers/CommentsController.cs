using Application.Features.Comments.DeleteComment;
using Application.Features.Comments.GetComments;
using Application.Features.Comments.UploadComment;
using Application.Features.Likes.LikeComment;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class CommentsController : BaseAuthApiController
    {
        public CommentsController(ISender mediator, IAuthenticatedUserService authenticatedUserService) : base(mediator, authenticatedUserService)
        {
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostComments(int postId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();
            IEnumerable<string> authUserRoles = GetCurrentUserRoles();

            var query = new GetPostCommentsQuery { 
                PostId = postId,
                AuthUserId = authUserId,
                AuthUserRoles = authUserRoles 
            };

            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost("{postId}")]
        public async Task<ActionResult<UploadCommentResponse>> UploadComment(int postId, [FromBody] string comment, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new UploadCommentCommand
            {
                PostId = postId,
                Content = comment,
                AuthUserId = authUserId, 
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{commentId}")]
        public async Task<ActionResult<DeleteCommentResponse>> DeleteComment(int commentId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();
            IEnumerable<string> authUserRoles = GetCurrentUserRoles();

            var command = new DeleteCommentCommand
            {
                CommentId = commentId,
                AuthUserId = authUserId,
                AuthUserRoles = authUserRoles
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpPost("{commentId}/like")]
        public async Task<IActionResult> LikeComment(int commentId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new LikeCommentCommand { 
                CommentId = commentId,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{commentId}/like")]
        public async Task<IActionResult> UnlikeComment(int commentId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new LikeCommentCommand
            {
                CommentId = commentId,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }
    }
}
