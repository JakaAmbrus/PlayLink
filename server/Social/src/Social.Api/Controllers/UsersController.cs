using Social.Application.Features.Users.Common;
using Social.Application.Features.Users.DeleteUser;
using Social.Application.Features.Users.EditUserDetails;
using Social.Application.Features.Users.GetNearestBirthdayUsers;
using Social.Application.Features.Users.GetUserById;
using Social.Application.Features.Users.GetUserByUsername;
using Social.Application.Features.Users.GetUsers;
using Social.Application.Features.Users.GetUsersForSearchBar;
using Social.Application.Features.Users.GetUsersUniqueCountries;
using Social.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Social.Api.Extensions;

namespace Social.Api.Controllers
{
    /// <summary>
    /// Manages users related operations.
    /// </summary>
    public class UsersController : BaseController
    {
        /// <summary>
        /// Returns all users based on filters: Age, Gender , Country, Activity and pagination parameters.
        /// </summary>
        /// <param name="userParams">Pagination parameters.</param>
        /// <returns>A paginated and filtered list of users DTOs.</returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams, CancellationToken cancellationToken)
        {
            var request = new GetUsersQuery 
            { 
                Params = userParams,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            Response.AddPaginationHeader(new PaginationHeader(response.Users.CurrentPage, response.Users.PageSize, response.Users.TotalCount, response.Users.TotalPages));
            return Ok(response);
        }

        /// <summary>
        /// Returns a list of users for the search bar.
        /// </summary>
        /// <returns>A DTO containing the users photo urls, genders, usernames, full names.</returns>
        [HttpGet("searchbar")]
        public async Task<IActionResult> GetUsersForSearchBar(CancellationToken cancellationToken)
        {
            var request = new GetUsersForSearchBarQuery();

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Returns a list of 3 users that are closest to their birthdays.
        /// </summary>
        /// <returns>A DTO containing users photo urls, genders, usernames, full names and birthdays with days until birthday integer.</returns>
        [HttpGet("nearest-birthday")]
        public async Task<IActionResult> GetNearestBirthdayUsers(CancellationToken cancellationToken)
        {
            var request = new GetNearestBirthdayUsersQuery();

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Returns a single user from the Database from the provided ID.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>A user DTO.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken)
        {
            var request = new GetUserByIdQuery { Id = id };

            var user = await Mediator.Send(request, cancellationToken);

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
            var request = new GetUserByUsernameQuery 
            { 
                Username = username,
                AuthUserId = AuthService.GetCurrentUserId(),
                AuthUserRoles = AuthService.GetCurrentUserRoles()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Returns a list of unique countries from the Database.
        /// </summary>
        /// <returns>A list of country names.</returns>
        [HttpGet("countries")]
        public async Task<IActionResult> GetUniqueCountries(CancellationToken cancellationToken)
        {
            var request = new GetUsersUniqueCountriesQuery { AuthUserId = AuthService.GetCurrentUserId() };

            var response = await Mediator.Send(request, cancellationToken);
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
            var request = new EditUserDetailsCommand
            {
                EditUserDto = editUserDto,
                AuthUserId = AuthService.GetCurrentUserId(),
                AuthUserRoles = AuthService.GetCurrentUserRoles()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Deletes a user from the Database, but a guest user is restricted.
        /// </summary>
        /// <returns>Confirmation of deletion.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
        {
            var request = new DeleteUserCommand
            {
                AuthUserId = AuthService.GetCurrentUserId(),
                AuthUserRoles = AuthService.GetCurrentUserRoles()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}

