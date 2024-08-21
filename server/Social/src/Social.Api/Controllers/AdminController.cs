using Social.Application.Features.Admin.AdminEditRoles;
using Social.Application.Features.Admin.AdminGetUsers;
using Social.Application.Features.Admin.AdminUserDelete;
using Social.Application.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social.Api.Extensions;

namespace Social.Api.Controllers
{
    /// <summary>
    /// Manages administrator actions
    /// </summary>
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : BaseController
    {
        /// <summary>
        /// Returns a list of users with their assigned roles.
        /// </summary>
        /// <param name="paginationParams">Parameters for pagination</param>
        /// <returns>List of user DTOs with the user ID, username, full name, roles and pictureURL/Gender(for the user display)</returns>
        [HttpGet("users")]
        public async Task<IActionResult> GetUsersWithRoles([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var request = new AdminGetUsersQuery
            {
                Params = paginationParams,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            Response.AddPaginationHeader(new PaginationHeader(response.Users.CurrentPage, response.Users.PageSize, response.Users.TotalCount, response.Users.TotalPages));
            return Ok(response);
        }

        /// <summary>
        /// Edits the roles of a user.
        /// </summary>
        /// <param name="UserId">User ID.</param>
        /// <returns>A confirmation of the edit success.</returns>
        [HttpPut("edit-roles/{userId:int}")]
        public async Task<ActionResult> EditRoles(int userId, CancellationToken cancellationToken)
        {        
            var request = new AdminEditRolesCommand
            {
                AppUserId = userId,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>A confirmation of the success of deletion.</returns>
        [HttpDelete("delete-user/{userId:int}")]
        public async Task<ActionResult> DeleteUser(int userId, CancellationToken cancellationToken)
        {
            var request = new AdminUserDeleteCommand
            {
                AppUserId = userId,
                AuthUserId = AuthService.GetCurrentUserId(),
                AuthUserRoles = AuthService.GetCurrentUserRoles()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
