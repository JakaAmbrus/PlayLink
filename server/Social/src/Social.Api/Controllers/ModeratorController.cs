using Social.Application.Features.Moderator.DeleteUserDescription;
using Social.Application.Features.Moderator.DeleteUserPhoto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Social.Api.Controllers
{
    /// <summary>
    /// Manages Moderator functions
    /// </summary>
    [Authorize(Policy = "RequireModeratorRole")] 
    
    public class ModeratorController : BaseController
    {
        /// <summary>
        /// Deletes the description of a user.
        /// </summary>
        /// <param name="username">Users username</param>
        /// <returns>A confirmation of deletion.</returns>
        [HttpDelete("delete-user-description/{username}")]
        public async Task<ActionResult> DeleteUserDescription(string username, CancellationToken cancellationToken)
        {
            var request = new DeleteUserDescriptionCommand { Username = username, };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Deletes the profile picture of a user and removes it from Cloudinary.
        /// </summary>
        /// <param name="Username">Users username.</param>
        /// <returns>A confirmation of deletion.</returns>
        [HttpDelete("delete-user-photo/{username}")]
        public async Task<ActionResult> DeleteUserPhoto(string username, CancellationToken cancellationToken)
        {
            var request = new DeleteUserPhotoCommand { Username = username, };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

    }
}
