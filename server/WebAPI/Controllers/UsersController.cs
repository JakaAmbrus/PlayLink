using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using MediatR;
using Application.Features.Users.GetUsers;
using Application.Features.Users.GetUserById;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Users.GetUserByUsername;
using Application.Features.Users.EditUserDetails;
using Application.Features.Authentication.Common;
using Application.Utils;
using WebAPI.Extensions;

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
        public async Task<ActionResult<PagedList<UserDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            var query = new GetUsersQuery { PaginationParams = userParams };
            var users = await _mediator.Send(query);

            Response.AddPaginationHeader(new PaginationHeader(users.Users.CurrentPage, users.Users.PageSize, users.Users.TotalCount, users.Users.TotalPages));

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

        [HttpPut("edit")]
        public async Task<ActionResult<EditUserDetailsResponse>> EditUserDetails([FromForm] EditUserDetailsCommand command)
        {
            var response = await _mediator.Send(command);

            if (response == null)
            {
                return BadRequest("Problem editing user");
            }

            return Ok(response);
        }
    }
}

