using Application.Features.Admin.AdminEditRoles;
using Application.Features.Admin.AdminGetUsers;
using Application.Features.Admin.AdminUserDelete;
using Application.Interfaces;
using Application.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Manages administrator actions
    /// </summary>
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : BaseAuthApiController
    {
        public AdminController(ISender mediator, IAuthenticatedUserService authenticatedUserService) : base(mediator, authenticatedUserService)
        {
        }

        /// <summary>
        /// Returns a list of users with their assigned roles.
        /// </summary>
        /// <param name="paginationParams">Parameters for pagination</param>
        /// <returns>List of user DTOs with the user ID, username, full name, roles and pictureURL/Gender(for the user display)</returns>
        [HttpGet("users")]
        public ActionResult GetUsersWithRoles([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var query = new AdminGetUsersQuery
            {
                Params = paginationParams,
                AuthUserId = authUserId
            };

            var result = Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Edits the roles of a user.
        /// </summary>
        /// <param name="UserId">User ID.</param>
        /// <param name="AssignModeratorRole">Boolean value that lets the controller know if the moderator role should be added or removed.</param>
        /// <returns>A confirmation of the edit success.</returns>
        [HttpPost("edit-roles/{UserId}")]
        public async Task<ActionResult> EditRoles(int UserId, [FromBody] bool AssignModeratorRole, CancellationToken cancellationToken)
        {        
            int authUserId = GetCurrentUserId();

            var command = new AdminEditRolesCommand
            {
                AppUserId = UserId,
                AssignModeratorRole = AssignModeratorRole,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="UserId">User ID.</param>
        /// <returns>A confirmation of the success of deletion.</returns>
        [HttpDelete("delete-user/{UserId}")]
        public async Task<ActionResult> DeleteUser(int UserId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();
            IEnumerable<string> authUserRoles = GetCurrentUserRoles();

            var command = new AdminUserDeleteCommand
            {
                AppUserId = UserId,
                AuthUserId = authUserId,
                AuthUserRoles = authUserRoles
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }
    }
}
