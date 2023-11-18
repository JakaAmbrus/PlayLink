using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using MediatR;
using Application.Features.Users.GetUsers;
using Application.Features.Users.GetUserById;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Users.GetUserByUsername;

namespace WebAPI.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Member")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
           var users = await _mediator.Send(new GetUsersQuery());

            if (users == null)
            {
                return NotFound("There are no users available");
            }

            return Ok(users);
        }

        [Authorize(Roles = "Member")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
           var user = await _mediator.Send(new GetUserByIdQuery(id));

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user); 
        }


        [HttpGet("username/{username}")]
        public async Task<ActionResult<AppUser>> GetUserByUsername(string username)
        {
            var user = await _mediator.Send(new GetUserByUsernameQuery(username));

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
    }
}

