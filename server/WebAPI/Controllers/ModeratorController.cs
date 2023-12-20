using Application.Features.Moderator.DeleteUserDescription;
using Application.Features.Moderator.DeleteUserPhoto;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Manages Moderator functions
    /// </summary>
    [Authorize(Policy = "RequireModeratorRole")] //Here I used the RequireModeratorRole policy only and intentionally do not perform extra checks
                                                 //in the handlers, for variety of approaches demonstration, more robust setup for Admin functions.
    public class ModeratorController : BaseAuthApiController
    {
        public ModeratorController(ISender mediator, IAuthenticatedUserService authenticatedUserService) : base(mediator, authenticatedUserService)
        {
        }

        /// <summary>
        /// Deletes the description of a user.
        /// </summary>
        /// <param name="Username">Users username</param>
        /// <returns>A confirmation of deletion.</returns>
        [HttpDelete("delete-user-description/{username}")]
        public async Task<ActionResult> DeleteUserDescription(string username, CancellationToken cancellationToken)
        {
            var command = new DeleteUserDescriptionCommand { Username = username, };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Deletes the profile picture of a user and removes it from Cloudinary.
        /// </summary>
        /// <param name="Username">Users username.</param>
        /// <returns>A confirmation of deletion.</returns>
        [HttpDelete("delete-user-photo/{username}")]
        public async Task<ActionResult> DeleteUserPhoto(string username, CancellationToken cancellationToken)
        {
            var command = new DeleteUserPhotoCommand { Username = username, };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

    }
}
