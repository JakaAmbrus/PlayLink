using Application.Features.Comments.DeleteComment;
using Application.Features.Comments.UploadComment;
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

        [HttpPost]
        public async Task<ActionResult<UploadCommentResponse>> UploadComment(UploadCommentCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpDelete("{commentId}")]
        public async Task<ActionResult<DeleteCommentResponse>> DeleteComment(int commentId)
        {
            var command = new DeleteCommentCommand(commentId);

            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
