using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using MediatR;
using Application.Features.Users.GetUsers;
using Application.Features.Users.GetUserById;

namespace WebAPI.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/AppUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
           var users = await _mediator.Send(new GetUsersQuery());

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        // GET: api/AppUsers/id
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
           var user = await _mediator.Send(new GetUserByIdQuery(id));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user); 
        }
    }
}

