using Application.Features.Likes.LikePost;
using Application.Features.Likes.UnlikePost;
using Application.Features.Posts.DeletePost;
using Application.Features.Posts.GetPostById;
using Application.Features.Posts.GetPosts;
using Application.Features.Posts.GetPostsByUser;
using Application.Features.Posts.GetUserPostPhotos;
using Application.Features.Posts.UploadPost;
using Application.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    public class PostsController : BaseApiController
    {
        private readonly ISender _mediator;

        public PostsController(ISender sender)
        {
            _mediator = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var query = new GetPostsQuery {Params = paginationParams };

            var posts = await _mediator.Send(query, cancellationToken);

            Response.AddPaginationHeader(new PaginationHeader(posts.Posts.CurrentPage, posts.Posts.PageSize, posts.Posts.TotalCount, posts.Posts.TotalPages));

            return Ok(posts);
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            var query = new GetPostByIdQuery(postId);

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("user/{username}")]
        public async Task<IActionResult> GetPostsByUser(string username, [FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var query = new GetPostsByUserQuery 
            {
                Username = username,
                Params = paginationParams
            };

            var result = await _mediator.Send(query, cancellationToken);

            Response.AddPaginationHeader(new PaginationHeader(result.Posts.CurrentPage, result.Posts.PageSize, result.Posts.TotalCount, result.Posts.TotalPages));

            return Ok(result);
        }

        [HttpGet("user/{username}/photos")]
        public async Task<IActionResult> GetUserPostPhotos(string username, CancellationToken cancellationToken)
        {
            var query = new GetUserPostPhotosQuery { Username = username };

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpPost]
        public async Task<ActionResult<UploadPostResponse>> UploadPost([FromForm] UploadPostCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpDelete("{postId}")]
        public async Task<ActionResult<Unit>> DeletePost(int postId, CancellationToken cancellationToken)
        {
            var command = new DeletePostCommand { PostId = postId };

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpPost("{postId}/like")]
        public async Task<IActionResult> LikePost(int postId, CancellationToken cancellationToken)
        {
            var command = new LikePostCommand { PostId = postId};

            var result = await _mediator.Send(command, cancellationToken);

            if (result.Liked)
            {
                return Ok(result);
            }

            return BadRequest("Unable to like the post.");
        }

        [HttpDelete("{postId}/like")]
        public async Task<IActionResult> UnlikePost(int postId, CancellationToken cancellationToken)
        {
            var command = new UnlikePostCommand { PostId = postId };

            var result = await _mediator.Send(command, cancellationToken);

            if (result.Unliked)
            {
                return Ok(result);
            }

            return BadRequest("Unable to unlike the post.");
        }
    }
}
