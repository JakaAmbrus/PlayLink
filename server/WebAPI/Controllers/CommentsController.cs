using Application.Features.Comments.Common;
using Application.Features.Comments.DeleteComment;
using Application.Features.Comments.GetComments;
using Application.Features.Comments.UploadComment;
using Application.Features.Likes.LikeComment;
using Application.Features.Likes.UnlikeComment;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize(Policy = "RequireMemberRole")]
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

            var query = new GetPostCommentsQuery(postId, authUserId, authUserRoles);

            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost("{postId}")]
        public async Task<ActionResult<UploadCommentResponse>> UploadComment(int postId, [FromBody] CommentContentDto comment, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new UploadCommentCommand(postId, comment, authUserId);

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{commentId}")]
        public async Task<ActionResult<DeleteCommentResponse>> DeleteComment(int commentId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();
            IEnumerable<string> authUserRoles = GetCurrentUserRoles();

            var command = new DeleteCommentCommand(commentId, authUserId, authUserRoles);

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpPost("{commentId}/like")]
        public async Task<IActionResult> LikeComment(int commentId, CancellationToken cancellationToken)
        {
            var command = new LikeCommentCommand { CommentId = commentId};

            var result = await Mediator.Send(command, cancellationToken);

            if (result.Liked)
            {
                return Ok(result);
            }
            
            return BadRequest("You have already liked this comment");
        }

        [HttpDelete("{commentId}/like")]
        public async Task<IActionResult> UnlikeComment(int commentId, CancellationToken cancellationToken)
        {
            var command = new UnlikeCommentCommand { CommentId = commentId };

            var result = await Mediator.Send(command, cancellationToken);

            if (result.Unliked)
            {
                return Ok(result);
            }

            return BadRequest("You have not liked this comment");
        }
    }
}
