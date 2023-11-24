using Application.Features.Comments.DeleteComment;
using Application.Features.Comments.GetComments;
using Application.Features.Comments.UploadComment;
using Application.Features.Likes.LikeComment;
using Application.Features.Likes.UnlikeComment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize(Policy = "RequireMemberRole")]
    public class CommentsController : BaseApiController
    {
        private readonly ISender _mediator;

        public CommentsController(ISender sender)
        {
            _mediator = sender;
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostComments(int postId, CancellationToken cancellationToken)
        {
            var query = new GetPostCommentsQuery { PostId = postId};

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<UploadCommentResponse>> UploadComment(UploadCommentCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{commentId}")]
        public async Task<ActionResult<DeleteCommentResponse>> DeleteComment(int commentId, CancellationToken cancellationToken)
        {
            var command = new DeleteCommentCommand(commentId);

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpPost("{commentId}/like")]
        public async Task<IActionResult> LikeComment(int commentId, CancellationToken cancellationToken)
        {
            var command = new LikeCommentCommand { CommentId = commentId};

            var result = await _mediator.Send(command, cancellationToken);

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

            var result = await _mediator.Send(command, cancellationToken);

            if (result.Unliked)
            {
                return Ok(result);
            }

            return BadRequest("You have not liked this comment");
        }
    }
}
