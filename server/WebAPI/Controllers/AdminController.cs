using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : ControllerBase
    {
    /*    private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }*/
        [HttpGet("users-with-roles")]
        public ActionResult GetUsersWithRoles()
        {
            return Ok("successful");
        }
/*        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, EditRolesDto editRolesDto)
        {
            editRolesDto.Username = username.ToLower();
            return Ok(await _mediator.Send(new EditRolesCommand { EditRolesDto = editRolesDto }));
        }*/
    }
}
