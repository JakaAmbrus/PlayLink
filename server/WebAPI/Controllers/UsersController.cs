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
    /// <summary>
    /// Manages users related operations.
    /// </summary>
    public class UsersController : BaseAuthApiController
    {
        public UsersController(ISender mediator, IAuthenticatedUserService authenticatedUserService) : base(mediator, authenticatedUserService)
        {
        }

        /// <summary>
        /// Returns all users based on filters: Age, Gender , Country, Activity and pagination parameters.
        /// </summary>
        /// <param name="paginationParams">Pagination parameters.</param>
        /// <returns>A paginated and filtered list of users DTOs.</returns>
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

        /// <summary>
        /// Returns a list of users for the search bar.
        /// </summary>
        /// <returns>A DTO containing the users photo urls, genders, usernames, full names.</returns>
        [HttpGet("searchbar")]
        public async Task<ActionResult<GetUsersForSearchBarResponse>> GetUsersForSearchBar(CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var query = new GetUsersForSearchBarQuery { AuthUserId = authUserId};

            var users = await Mediator.Send(query, cancellationToken);

            return Ok(users);
        }

        /// <summary>
        /// Returns a single user from the Database from the provided ID.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>A user DTO.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserByIdResponse>> GetUser(int id, CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery { Id = id };

            var user = await Mediator.Send(query, cancellationToken);

            return Ok(user); 
        }

        /// <summary>
        /// Returns a single user information from the Database from the provided username.
        /// </summary>
        /// <param name="username">User username</param>
        /// <returns>A DTO of user details for their profile page.</returns>
        [HttpGet("username/{username}")]
        public async Task<ActionResult<AppUser>> GetUserByUsername(string username, CancellationToken cancellationToken)
        {
            var query = new GetUserByUsernameQuery { Username = username };

            var response = await Mediator.Send(query, cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Returns a list of unique countries from the Database.
        /// </summary>
        /// <returns>A list of country names.</returns>
        [HttpGet("countries")]
        public async Task<IActionResult> GetUniqueCountries(CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var query = new GetUsersUniqueCountriesQuery { AuthUserId = authUserId };

            var response = await Mediator.Send(query, cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Edits user details.
        /// </summary>
        /// <param name="editUserDto">Description, Country or Photo file</param>
        /// <returns>Updated user information: Description, Country or PhotoUrl</returns>
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

