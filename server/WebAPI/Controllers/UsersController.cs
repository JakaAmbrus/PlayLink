using Application.Features.Users.Common;
using Application.Features.Users.DeleteUser;
using Application.Features.Users.EditUserDetails;
using Application.Features.Users.GetNearestBirthdayUsers;
using Application.Features.Users.GetUserById;
using Application.Features.Users.GetUserByUsername;
using Application.Features.Users.GetUsers;
using Application.Features.Users.GetUsersForSearchBar;
using Application.Features.Users.GetUsersUniqueCountries;
using Application.Interfaces;
using Application.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

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
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams, CancellationToken cancellationToken)
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
        public async Task<IActionResult> GetUsersForSearchBar(CancellationToken cancellationToken)
        {
            var query = new GetUsersForSearchBarQuery { };

            var users = await Mediator.Send(query, cancellationToken);

            return Ok(users);
        }

        /// <summary>
        /// Returns a list of 3 users that are closest to their birthdays.
        /// </summary>
        /// <returns>A DTO containing users photo urls, genders, usernames, full names and birthdays with days until birthday integer.</returns>
        [HttpGet("nearest-birthday")]
        public async Task<IActionResult> GetNearestBirthdayUsers(CancellationToken cancellationToken)
        {
            var query = new GetNearestBirthdayUsersQuery { };

            var users = await Mediator.Send(query, cancellationToken);

            return Ok(users);
        }

        /// <summary>
        /// Returns a single user from the Database from the provided ID.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>A user DTO.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken)
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
        public async Task<IActionResult> GetUserByUsername(string username, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();
            IEnumerable<string> authUserRoles = GetCurrentUserRoles();

            var query = new GetUserByUsernameQuery 
            { 
                Username = username,
                AuthUserId = authUserId,
                AuthUserRoles = authUserRoles
            };

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
        public async Task<IActionResult> EditUserDetails([FromForm] EditUserDto editUserDto, CancellationToken cancellationToken)
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

        /// <summary>
        /// Deletes a user from the Database, but a guest user is restricted.
        /// </summary>
        /// <returns>Confirmation of deletion.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();
            IEnumerable<string> authUserRoles = GetCurrentUserRoles();

            var command = new DeleteUserCommand
            {
                AuthUserId = authUserId,
                AuthUserRoles = authUserRoles
            };

            var response = await Mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}

