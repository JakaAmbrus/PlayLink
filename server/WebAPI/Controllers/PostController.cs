﻿using Application.Features.Posts.DeletePost;
using Application.Features.Posts.GetPostById;
using Application.Features.Posts.GetPosts;
using Application.Features.Posts.GetPostsByUser;
using Application.Features.Posts.UploadPost;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class PostController : BaseApiController
    {
        private readonly ISender _mediator;

        public PostController(ISender sender)
        {
            _mediator = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var query = new GetPostsQuery();

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            var query = new GetPostByIdQuery(postId);

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPostsByUser(int userId)
        {
            var query = new GetPostsByUserQuery(userId);

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<UploadPostResponse>> UploadPost(UploadPostCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpDelete("{postId}")]
        public async Task<ActionResult<Unit>> DeletePost(int postId)
        {
            var command = new DeletePostCommand(postId);

            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
