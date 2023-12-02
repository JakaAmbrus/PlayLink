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
using Application.Features.Users.GetUsersUniqueCountries;
using Application.Features.Users.GetUsersForSearchBar;
using Application.Interfaces;
using Application.Features.Users.Common;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "Member")]
    public class UsersController : BaseAuthApiController
    {
        public UsersController(ISender mediator, IAuthenticatedUserService authenticatedUserService) : base(mediator, authenticatedUserService)
        {
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<UserDto>>> GetUsers([FromQuery] UserParams userParams, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var query = new GetUsersQuery 
            { 
                Params = userParams,
                AuthUserId = authUserId
            };

            var users = await Mediator.Send(query, cancellationToken);

            Response.AddPaginationHeader(new PaginationHeader(users.Users.CurrentPage, users.Users.PageSize, users.Users.TotalCount, users.Users.TotalPages));

            return Ok(users);
        }

        [HttpGet("searchbar")]
        public async Task<ActionResult<GetUsersForSearchBarResponse>> GetUsersForSearchBar(CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var query = new GetUsersForSearchBarQuery { AuthUserId = authUserId};

            var users = await Mediator.Send(query, cancellationToken);

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserByIdResponse>> GetUser(int id, CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery { Id = id };

            var user = await Mediator.Send(query, cancellationToken);

            return Ok(user); 
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<AppUser>> GetUserByUsername(string username, CancellationToken cancellationToken)
        {
            var query = new GetUserByUsernameQuery { Username = username };

            var response = await Mediator.Send(query, cancellationToken);

            return Ok(response);
        }

        [HttpGet("countries")]
        public async Task<IActionResult> GetUniqueCountries(CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var query = new GetUsersUniqueCountriesQuery { AuthUserId = authUserId };

            var response = await Mediator.Send(query, cancellationToken);

            return Ok(response);
        }

        [HttpPut("edit")]
        public async Task<ActionResult<EditUserDetailsResponse>> EditUserDetails([FromForm] EditUserDto editUserDto, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();
            IEnumerable<string> authUserRoles = GetCurrentUserRoles();

            var command = new EditUserDetailsCommand
            {
                EditUserDto = editUserDto,
                AuthUserId = authUserId,
                AuthUserRoles = authUserRoles
            };

            var response = await Mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}

